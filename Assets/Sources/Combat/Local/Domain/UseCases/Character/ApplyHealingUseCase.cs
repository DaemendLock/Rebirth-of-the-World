using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct ApplyHealingUseCase
    {
        private readonly IHealthRepository _healthRepository;
        private readonly IHealthOutput _healthOutput;

        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IStatusRepository _statusRepository;

        public ApplyHealingUseCase(IHealthRepository healthRepository, IHealthOutput healthOutput, IStatusOwnerRepository statusOwnerRepository, IStatusRepository statusRepository)
        {
            _healthRepository = healthRepository;
            _healthOutput = healthOutput;
            _statusOwnerRepository = statusOwnerRepository;
            _statusRepository = statusRepository;
        }

        public void Execute(EntityId target, float healing, HealingFlags flags, EntityId? healer, EventSource source)
        {
            HealingInstance instance = CreateHealingInstance(target, healing, flags, healer, source);

            Health health = _healthRepository.Get(target);
            healing = instance.Healing;

            health.TakeHealing(healing);

            _healthRepository.Update(health);
            _healthOutput.Present(health);

            if (instance.Flags.HasFlag(HealingFlags.CanRevive) && health.CurrentHealth > 0)
            {
                //Revive(new(data.Healer, data.Source, ReviveFlags.Healed));
            }

            if (instance.Flags.HasFlag(HealingFlags.NonReactable) == false)
            {
                HandleEvent();
                //_applyHealingEventHandler.HandleEvent(result);
            }
        }

        private HealingInstance CreateHealingInstance(EntityId targetId, float healing, HealingFlags flags, EntityId? healer, EventSource source)
        {
            HealingInstance result = new(targetId, healing, flags, healer, source);
            HealingModification finalModification = new(0, 0, 0, HealingFlags.None);

            if (healer.HasValue)
            {
                var ids = _statusOwnerRepository.Get(healer.Value);

                foreach (var id in ids.GetAll())
                {
                    if (_statusRepository.TryGet(id, out Status status) == false)
                    {
                        continue;
                    }

                    if (status.Strategy.TryGetEffect(out ModifyOutgoingHealingEffect effect) == false)
                    {
                        continue;
                    }

                    HealingModification modification = effect.GetModification(result);
                    finalModification = new(finalModification.BaseValue + modification.BaseValue,
                            finalModification.PercentModication + modification.PercentModication,
                            finalModification.BonusValue + modification.BonusValue,
                            finalModification.FlagsModification | modification.FlagsModification);
                }
            }

            result.Healing = (result.OriginalHealing + finalModification.BaseValue) * 100f / (100 + finalModification.PercentModication) + finalModification.BonusValue;
            result.Flags |= finalModification.FlagsModification;
            return result;
        }

        private void HandleEvent()
        {

        }
    }
}
