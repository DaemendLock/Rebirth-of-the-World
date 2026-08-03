using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct HealthApplyDamageUseCase
    {
        private readonly IHealthRepository _healthRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IStatusRepository _statusRepository;

        private readonly IHealthOutput _healthOutput;
        private readonly ICharacterConsciousStateOutput _characterConsciousStateOutput;

        public HealthApplyDamageUseCase(IHealthRepository healthRepository, IHealthOutput healthOutput, IActorRepository stateRepository, IStatusOwnerRepository statusOwnerRepository, IStatusRepository statusRepository, ICharacterConsciousStateOutput characterConsciousStateOutput)
        {
            _healthRepository = healthRepository;
            _healthOutput = healthOutput;
            _actorRepository = stateRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _statusRepository = statusRepository;
            _characterConsciousStateOutput = characterConsciousStateOutput;
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

            DamageModification defenderModification = GetTargetDamageModification(instance.Target, instance);
            DamageModification attackerModification = default;

            if (instance.Attacker.HasValue)
            {
                attackerModification = GetAttackerDamageModification(instance.Attacker.Value, instance);
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

        private DamageModification GetTargetDamageModification(UnitId target, DamageInstance instance)
        {
            DamageModification result = new(0, 0, 0, DamageFlags.None);
            StatusOwner statuses = _statusOwnerRepository.Get(target);

            foreach (StatusId id in statuses.GetAll())
            {
                if (_statusRepository.TryGet(id, out Status status) == false)
                {
                    continue;
                }

                if (status.Properties.TryGetProperty(out IModifyParentIncomingDamageStrategy effect) == false)
                {
                    continue;
                }

                result += effect.GetModification(instance);
            }

            return result;
        }

        private DamageModification GetAttackerDamageModification(UnitId atacker, DamageInstance instance)
        {
            DamageModification result = new(0, 0, 0, DamageFlags.None);
            StatusOwner statuses = _statusOwnerRepository.Get(atacker);

            foreach (StatusId id in statuses.GetAll())
            {
                if (_statusRepository.TryGet(id, out Status status) == false)
                {
                    continue;
                }

                if (status.Properties.TryGetProperty(out IModifyParentOutgoingDamageStrategy effect) == false)
                {
                    continue;
                }

                result += effect.GetModification(instance);
            }

            return result;
        }

        private void HandleEvent(DamageResult @event)
        {
            StatusOwner targetStatuses = _statusOwnerRepository.Get(@event.Target);

            foreach (StatusId statusId in targetStatuses.GetAll())
            {
                if (_statusRepository.TryGet(statusId, out Status status) == false)
                {
                    continue;
                }

                if (status.Properties.TryGetProperty(out ITakeDamageEffectStrategy effect) == false)
                {
                    continue;
                }

                effect.HandleDamage(@event);
            }

            if (@event.Attacker.HasValue == false)
            {
                return;
            }

            StatusOwner attackerStatuses = _statusOwnerRepository.Get(@event.Attacker.Value);

            foreach (var statusId in attackerStatuses.GetAll())
            {
                if (_statusRepository.TryGet(statusId, out Status status) == false)
                {
                    continue;
                }

                if (status.Properties.TryGetProperty(out IDealDamageEffectStrategy effect) == false)
                {
                    continue;
                }

                effect.HandleDamage(@event);
            }
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
