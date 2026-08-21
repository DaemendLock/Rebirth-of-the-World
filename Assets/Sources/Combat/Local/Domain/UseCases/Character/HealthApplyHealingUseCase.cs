using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using static UnityEngine.Analytics.IAnalytic;
using static UnityEngine.GraphicsBuffer;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct HealthApplyHealingUseCase
    {
        private readonly IHealthRepository _healthRepository;
        private readonly IHealthOutput _healthOutput;

        private readonly IStatusOwnerRepository _statusOwnerRepository;

        private readonly IHealingDamageModifierCalculator _damageModifierCalculator;

        public HealthApplyHealingUseCase(IHealthRepository healthRepository, IHealthOutput healthOutput, IStatusOwnerRepository statusOwnerRepository, IHealingDamageModifierCalculator damageModifierCalculator)
        {
            _healthRepository = healthRepository;
            _healthOutput = healthOutput;
            _statusOwnerRepository = statusOwnerRepository;
            _damageModifierCalculator = damageModifierCalculator;
        }

        public void Execute(UnitId target, float healing, HealingFlags flags, UnitId? healer, AbilityKey? source)
        {
            if (_healthRepository.TryGet(target, out HealthOwner health) == false)
            {
                throw new System.InvalidOperationException();
            }

            HealingInstance instance = CreateHealingInstance(target, healing, flags, healer, source);
            healing = instance.Healing;

            health.TakeHealing(healing);

            _healthRepository.Update(health);
            _healthOutput.Present(health);

            if (instance.Flags.HasFlag(HealingFlags.CanRevive) && health.CurrentValue > 0)
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
                StatusOwner healerStatuses = _statusOwnerRepository.Get(healer.Value);

                finalModification += _damageModifierCalculator.GetHealingModification(healerStatuses.GetAll(), result);
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
