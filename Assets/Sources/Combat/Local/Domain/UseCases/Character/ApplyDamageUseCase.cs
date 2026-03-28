using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct ApplyDamageUseCase
    {
        private readonly IHealthRepository _healthRepository;
        private readonly IStateRepository _stateRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IStatusRepository _statusRepository;

        private readonly IHealthOutput _healthOutput;
        private readonly ICharacterConsciousStateOutput _characterConsciousStateOutput;

        public ApplyDamageUseCase(IHealthRepository healthRepository, IHealthOutput healthOutput, IStateRepository stateRepository, IStatusOwnerRepository statusOwnerRepository, IStatusRepository statusRepository, ICharacterConsciousStateOutput characterConsciousStateOutput)
        {
            _healthRepository = healthRepository;
            _healthOutput = healthOutput;
            _stateRepository = stateRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _statusRepository = statusRepository;
            _characterConsciousStateOutput = characterConsciousStateOutput;
        }

        //public void Execute(DamageInstanceId damageInstanceId)
        //{
        //    DamageInstance instance = _damageInstanceRepository2.Get(damageInstanceId);
        //    ApplyDamageInstance(instance);
        //}

        public void Execute(EntityId targetId, float damage, DamageFlags flags, EntityId? attacker, EventSource source)
        {
            DamageInstance instance = CreateDamageInstance(targetId, damage, flags, attacker, source);
            ApplyDamageInstance(instance);
        }

        private DamageInstance CreateDamageInstance(EntityId targetId, float damage, DamageFlags flags, EntityId? attacker, EventSource source)
        {
            DamageInstance result = new(targetId, damage, flags, attacker, source);
            DamageModification finalModification = new(0, 0, 0, DamageFlags.None);

            StatusOwner defenderStatuses = _statusOwnerRepository.Get(targetId);

            foreach (StatusId id in defenderStatuses.GetAll())
            {
                if (_statusRepository.TryGet(id, out Status status) == false)
                {
                    continue;
                }

                if (status.Strategy.TryGetEffect(out ModifyIncomingDamageEffect effect) == false)
                {
                    continue;
                }

                DamageModification modification = effect.GetModification(result);
                finalModification = new(finalModification.BaseValue + modification.BaseValue,
                        finalModification.PercentModication + modification.PercentModication,
                        finalModification.BonusValue + modification.BonusValue,
                        finalModification.FlagsModification | modification.FlagsModification);
            }

            if (attacker.HasValue)
            {
                var ids = _statusOwnerRepository.Get(attacker.Value);

                foreach (var id in ids.GetAll())
                {
                    if (_statusRepository.TryGet(id, out Status status) == false)
                    {
                        continue;
                    }

                    if (status.Strategy.TryGetEffect(out ModifyOutgoingDamageEffect effect) == false)
                    {
                        continue;
                    }

                    DamageModification modification = effect.GetModification(result);
                    finalModification = new(finalModification.BaseValue + modification.BaseValue,
                            finalModification.PercentModication + modification.PercentModication,
                            finalModification.BonusValue + modification.BonusValue,
                            finalModification.FlagsModification | modification.FlagsModification);
                }
            }

            result.Damage = (result.Damage + finalModification.BaseValue) * 100f / (100 + finalModification.PercentModication) + finalModification.BonusValue;
            result.Flags |= finalModification.FlagsModification;
            return result;
        }

        private void ApplyDamageInstance(DamageInstance instance)
        {
            Health health = _healthRepository.Get(instance.Target);
            float finalDamage = instance.Damage;

            if (finalDamage >= health.CurrentHealth && instance.Flags.HasFlag(DamageFlags.NonLethal))
            {
                finalDamage = health.CurrentHealth - 1;
            }

            health.TakeDamage(finalDamage);

            _healthRepository.Update(health);
            _healthOutput.Present(health);

            if (instance.Flags.HasFlag(DamageFlags.NonReactable) == false)
            {
                DamageResult result = new(instance.Target, instance.OriginalDamage, finalDamage, instance.Flags, instance.Attacker, instance.Source.Skill, instance.Source.Unit);
                HandleEvent(result);
            }

            if (health.CurrentHealth <= 0)
            {
                //if (instance.Flags.HasFlag(DamageFlags.InstantKill))
                //{
                Kill(health.Id, instance.Attacker, instance.Source);
                //}
                //else
                //{
                //    KnockDown(health.Id, instance.Attacker, instance.Source);
                //}
            }
        }

        private void HandleEvent(DamageResult @event)
        {
            StatusOwner targetStatuses = _statusOwnerRepository.Get(@event.Target);

            foreach (var statusId in targetStatuses.GetAll())
            {
                if (_statusRepository.TryGet(statusId, out Status status) == false)
                {
                    continue;
                }

                if (status.Strategy.TryGetEffect(out TakeDamageEffect effect) == false)
                {
                    continue;
                }

                effect.Handle(@event);
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

                if (status.Strategy.TryGetEffect(out DealDamageEffect effect) == false)
                {
                    continue;
                }

                effect.Handle(@event);
            }
        }

        private void Kill(EntityId target, EntityId? attacker, EventSource source)
        {
            CharacterState state = _stateRepository.Get(target);

            if (state.ConsciousState == ConsciousState.Dead)
            {
                return;
            }

            state.ConsciousState = ConsciousState.Dead;
            _stateRepository.Update(state);
            _characterConsciousStateOutput.Present(target, state.ConsciousState);
            return;
        }
    }
}
