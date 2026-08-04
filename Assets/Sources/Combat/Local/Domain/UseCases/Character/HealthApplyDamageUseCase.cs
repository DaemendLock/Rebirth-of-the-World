using Combat.Common.Flags;
using Combat.Common.ValueObjects;
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

        public HealthApplyDamageUseCase(IHealthRepository healthRepository, IHealthOutput healthOutput, IActorRepository stateRepository, IStatusOwnerRepository statusOwnerRepository, ICharacterConsciousStateOutput characterConsciousStateOutput, IHealingDamageModifierCalculator damageModifierCalculator, IDamageResultHandler damageResultHandler)
        {
            _healthRepository = healthRepository;
            _healthOutput = healthOutput;
            _actorRepository = stateRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _characterConsciousStateOutput = characterConsciousStateOutput;
            _damageModifierCalculator = damageModifierCalculator;
            _damageResultHandler = damageResultHandler;
        }

        public void Execute(UnitId targetId, float damage, DamageFlags flags, UnitId? attacker, AbilityKey? source)
        {
            if (_healthRepository.TryGet(targetId, out Health health) == false)
            {
                throw new System.InvalidOperationException();
            }

            DamageInstance instance = CreateInstance(targetId, damage, flags, attacker, source);

            float finalDamage = ApplyDamageInstance(instance, ref health);
            TriggerEffects(instance, finalDamage);
            UpdateConsciosState(instance, ref health);
        }

        private DamageInstance CreateInstance(UnitId targetId, float damage, DamageFlags flags, UnitId? attacker, AbilityKey? source)
        {
            DamageInstance instance = new(targetId, damage, flags, attacker, source);

            System.ReadOnlySpan<StatusId> defenderStatuses = _statusOwnerRepository.Get(targetId).GetAll();
            DamageModification defenderModification = _damageModifierCalculator.GetDefenderDamageModification(defenderStatuses, instance);
            DamageModification attackerModification = default;

            if (instance.Attacker.HasValue)
            {
                System.ReadOnlySpan<StatusId> attackerStatuses = _statusOwnerRepository.Get(instance.Attacker.Value).GetAll();
                attackerModification = _damageModifierCalculator.GetAttackerDamageModification(attackerStatuses, instance);
            }

            DamageModification finalModification = defenderModification + attackerModification;

            instance.Damage = (instance.Damage + finalModification.BaseValue) * 100f / (100 + finalModification.PercentModication) + finalModification.BonusValue;
            instance.Flags |= finalModification.FlagsModification;
            return instance;
        }

        private float ApplyDamageInstance(DamageInstance instance, ref Health health)
        {
            float finalDamage = instance.Damage;

            if (finalDamage >= health.CurrentHealth && instance.Flags.HasFlag(DamageFlags.NonLethal))
            {
                finalDamage = health.CurrentHealth - 1;
            }

            health.TakeDamage(finalDamage);

            //_healthRepository.Update(health);
            _healthOutput.Present(health);

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

        private void UpdateConsciosState(DamageInstance instance, ref Health health)
        {
            if (health.CurrentHealth > 0)
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
            StatusOwner targetStatuses = _statusOwnerRepository.Get(@event.Target);
            _damageResultHandler.HandleDamageRecieved(targetStatuses.GetAll(), @event);

            if (@event.Attacker.HasValue == false)
            {
                return;
            }

            StatusOwner attackerStatuses = _statusOwnerRepository.Get(@event.Attacker.Value);
            _damageResultHandler.HandleDamageDealth(attackerStatuses.GetAll(), @event);
        }

        private void Kill(UnitId target, UnitId? attacker, AbilityKey? source)
        {
            Actor actor = _actorRepository.Get(target);

            if (actor.ConsciousState == ConsciousState.Dead)
            {
                return;
            }

            actor.Kill();
            _actorRepository.Update(actor);
            _characterConsciousStateOutput.Present(target, actor.ConsciousState);
            return;
        }
    }
}
