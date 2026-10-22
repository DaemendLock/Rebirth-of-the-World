using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Application.ValueObjects;
using Combat.Local.Domain.Repositories;

using System.Linq;

namespace Combat.Local.Application.UseCases
{
    public sealed class HealthApplyHealingUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IHealthOutput _healthOutput;

        private readonly IHealingDamageModificationCalculator _damageModifierCalculator;

        public HealthApplyHealingUseCase(IUnitStore units, IHealthOutput healthOutput, IHealingDamageModificationCalculator damageModifierCalculator)
        {
            _unitStore = units;
            _healthOutput = healthOutput;

            _damageModifierCalculator = damageModifierCalculator;
        }

        public void Execute(UnitId target, float healing, HealingFlags flags, UnitId? healer, AbilityKey? source)
        {
            ref HealthOwner health = ref _unitStore.Get(target).Health;
            float originalHealth = health.Current;
            HealingInstance instance = CreateHealingInstance(target, healing, flags, healer, source);
            healing = instance.Healing;

            health.TakeHealing(healing);

            _healthOutput.Present(new HealthChangeRecord(health.Id, health.Current, health.Max, originalHealth));

            if (instance.Flags.HasFlag(HealingFlags.CanRevive) && health.Current > 0)
            {
                //Revive(new(data.Healer, data.Source, ReviveFlags.Healed));
            }

            if (instance.Flags.HasFlag(HealingFlags.NonReactable) == false)
            {
                HandleEvent();
                //_applyHealingEventHandler.HandleEvent(result);
            }
        }

        private HealingInstance CreateHealingInstance(UnitId targetId, float healing, HealingFlags flags, UnitId? healerId, AbilityKey? source)
        {
            HealingInstance result = new(targetId, healing, flags, healerId, source);
            HealingModification finalModification = new(0, 100f, 0, HealingFlags.None);

            if (healerId.HasValue)
            {
                try
                {
                    ref UnitContext healer = ref _unitStore.Get(healerId.Value);
                    finalModification += _damageModifierCalculator.GetOutgoingHealingModification(healer.Statuses.GetAll().Select(value => value.Id), result);
                }
                catch
                {
                    return result;
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
