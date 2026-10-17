using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Application.ValueObjects;

using System.Linq;

namespace Combat.Local.Application.UseCases
{
    public sealed class HealthApplyHealingUseCase
    {
        private readonly IHealthOwnerRepository _healthRepository;
        private readonly IHealthOutput _healthOutput;

        private readonly IStatusOwnerRepository _statusOwnerRepository;

        private readonly IHealingDamageModificationCalculator _damageModifierCalculator;

        public HealthApplyHealingUseCase(IHealthOwnerRepository healthRepository, IHealthOutput healthOutput, IStatusOwnerRepository statusOwnerRepository, IHealingDamageModificationCalculator damageModifierCalculator)
        {
            _healthRepository = healthRepository;
            _healthOutput = healthOutput;
            _statusOwnerRepository = statusOwnerRepository;
            _damageModifierCalculator = damageModifierCalculator;
        }

        public void Execute(UnitId target, float healing, HealingFlags flags, UnitId? healer, AbilityKey? source)
        {
            ref HealthOwner health = ref _healthRepository.Get(target);
            float originalHealth = health.CurrentHealth;
            HealingInstance instance = CreateHealingInstance(target, healing, flags, healer, source);
            healing = instance.Healing;

            health.TakeHealing(healing);

            _healthOutput.Present(new HealthChangeRecord(health.Id, health.CurrentHealth, health.MaxHealth, originalHealth));

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

        private HealingInstance CreateHealingInstance(UnitId targetId, float healing, HealingFlags flags, UnitId? healer, AbilityKey? source)
        {
            HealingInstance result = new(targetId, healing, flags, healer, source);
            HealingModification finalModification = new(0, 100f, 0, HealingFlags.None);

            if (healer.HasValue)
            {
                if (_statusOwnerRepository.TryGet(healer.Value, out StatusOwner healerStatuses) == false)
                {
                    return result;
                }

                finalModification += _damageModifierCalculator.GetOutgoingHealingModification(healerStatuses.GetAll().Select(value => value.Id), result);
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
