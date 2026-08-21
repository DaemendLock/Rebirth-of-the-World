using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct HealthApplyDamageUseCase
    {
        private readonly IHealthRepository _healthRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;

        private readonly IHealthOutput _healthOutput;
        private readonly ICharacterConsciousStateOutput _characterConsciousStateOutput;
        private readonly IHealingDamageModifierCalculator _damageModifierCalculator;
        private readonly IDamageResultHandler _damageResultHandler;

        private readonly ICharacterDeathHandler _characterDeathHandler;

        public HealthApplyDamageUseCase(IHealthRepository healthRepository, IHealthOutput healthOutput, IActorRepository stateRepository, IStatusOwnerRepository statusOwnerRepository, ICharacterConsciousStateOutput characterConsciousStateOutput, IHealingDamageModifierCalculator damageModifierCalculator, IDamageResultHandler damageResultHandler, ICharacterDeathHandler characterDeathHandler)
        {
            _healthRepository = healthRepository;
            _healthOutput = healthOutput;
            _actorRepository = stateRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _characterConsciousStateOutput = characterConsciousStateOutput;
            _damageModifierCalculator = damageModifierCalculator;
            _damageResultHandler = damageResultHandler;
            _characterDeathHandler = characterDeathHandler;
        }

        public void Execute(UnitId target, float damage, DamageFlags flags, UnitId? attacker, AbilityKey? source)
        {
            if (_healthRepository.TryGet(target, out HealthOwner health) == false)
            {
                throw new System.InvalidOperationException();
            }

            DamageInstance instance = CreateInstance(target, damage, flags, attacker, source);

            float finalDamage = ApplyDamageInstance(instance, health);
            TriggerEffects(instance, finalDamage);
            UpdateConsciosState(instance);
        }

        private DamageInstance CreateInstance(UnitId target, float damage, DamageFlags flags, UnitId? attacker, AbilityKey? source)
        {
            DamageInstance instance = new(target, damage, flags, attacker, source);
            DamageModification defenderModification = default;
            DamageModification attackerModification = default;

            StatusOwner statusOwner = _statusOwnerRepository.Get(target);
            defenderModification = _damageModifierCalculator.GetDefenderDamageModification(statusOwner.GetAll(), instance);

            if (attacker.HasValue)
            {
                StatusOwner attackerStatuses = _statusOwnerRepository.Get(attacker.Value);
                attackerModification = _damageModifierCalculator.GetAttackerDamageModification(attackerStatuses.GetAll(), instance);
            }

            DamageModification finalModification = defenderModification + attackerModification;

            instance.Damage = (instance.Damage + finalModification.BaseValue) * 100f / (100 + finalModification.PercentModication) + finalModification.BonusValue;
            instance.Flags |= finalModification.FlagsModification;
            return instance;
        }

        private float ApplyDamageInstance(DamageInstance instance, HealthOwner health)
        {
            float finalDamage = instance.Damage;

            if (finalDamage >= health.CurrentValue && instance.Flags.HasFlag(DamageFlags.NonLethal))
            {
                finalDamage = health.CurrentValue - 1;
            }

            health.TakeDamage(finalDamage);

            _healthRepository.Update(health);
            _healthOutput.Present(instance);

            return finalDamage;
        }

        private void TriggerEffects(DamageInstance instance, float finalDamage)
        {
            if (instance.Flags.HasFlag(DamageFlags.NonReactable))
            {
                return;
            }

            DamageResult result = new(instance.Target, instance.OriginalDamage, finalDamage, instance.Flags, instance.Attacker, instance.Source);
            HandleEvent(result);
        }

        private void UpdateConsciosState(DamageInstance instance)
        {
            if (_healthRepository.TryGet(instance.Target, out HealthOwner health) == false)
            {
                return;
            }

            if (health.CurrentValue > 0)
            {
                return;

            }
            //if (instance.Flags.HasFlag(DamageFlags.InstantKill))
            //{
            Kill(health.Id, instance.Attacker, instance.Source);
            //}
            //else
            //{
            //    KnockDown(health.Id, instance.Attacker, instance.Source);
            //}
        }

        private void HandleEvent(DamageResult @event)
        {
            StatusOwner statusOwner = _statusOwnerRepository.Get(@event.Target);
            _damageResultHandler.HandleDamageRecieved(statusOwner.GetAll(), @event);

            if (@event.Attacker.HasValue)
            {
                StatusOwner attackerStatuses = _statusOwnerRepository.Get(@event.Attacker.Value);
                _damageResultHandler.HandleDamageDealth(attackerStatuses.GetAll(), @event);
            }
        }

        private void Kill(UnitId target, UnitId? attacker, AbilityKey? source)
        {
            if (_actorRepository.TryGet(target, out Actor actor) == false)
            {
                return;
            }

            if (actor.ConsciousState == ConsciousState.Dead)
            {
                return;
            }

            actor.Kill();
            _actorRepository.Update(actor);
            _characterDeathHandler.Handle(new(target));
            _characterConsciousStateOutput.Present(target, actor.ConsciousState);
            return;
        }
    }
}
