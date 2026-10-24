using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.UseCases;

namespace Combat.Local.Application.Facades
{
    public readonly struct HealthOwnerFacade
    {
        private readonly ComposureSetUseCase _setComposureUseCase;
        private readonly HealthSetUseCase _setHealthUseCase;
        private readonly UnitApplyDamageUseCase _applyDamageUseCase;
        private readonly HealthApplyHealingUseCase _applyHealingUseCase;

        private readonly IUnitStore _unitStore;

        public HealthOwnerFacade(HealthSetUseCase setHealthUseCase, UnitApplyDamageUseCase applyDamageUseCase, HealthApplyHealingUseCase applyHealingUseCase, IUnitStore unitStore, ComposureSetUseCase composureSetUseCase)
        {
            _setHealthUseCase = setHealthUseCase;
            _applyDamageUseCase = applyDamageUseCase;
            _applyHealingUseCase = applyHealingUseCase;
            _unitStore = unitStore;
            _setComposureUseCase = composureSetUseCase;
        }

        public HealthValueDTO GetHealth(UnitId entityId)
        {
            UnitContext value = _unitStore.Get(entityId);
            return new(value.Health.Current, value.Health.Max, value.Composure.Current, value.Composure.Max);
        }

        public void ApplyDamage(ApplyDamageInfo info) => _applyDamageUseCase.Execute(info);

        public void ApplyHealing(ApplyHealingInfo info) => _applyHealingUseCase.Execute(info.Target, info.OriginalHealing, info.Flags, info.Healer, info.Source);

        public void SetComposure(UnitId target, float value) => _setComposureUseCase.Execute(target, value);

        public void SetHealth(UnitId target, float value) => _setHealthUseCase.Execute(target, value);
    }
}

namespace Combat.Local.Application.Facades
{
    public readonly struct HealthValueDTO
    {
        public HealthValueDTO(float currentHealth, float maxHealth, float currentComposure, float maxComposure)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            CurrentComposure = currentComposure;
            MaxComposure = maxComposure;
        }

        public float CurrentHealth { get; }
        public float MaxHealth { get; }
        public float CurrentComposure { get; }
        public float MaxComposure { get; }
    }

    public readonly ref struct ApplyHealingInfo
    {
        public ApplyHealingInfo(UnitId target, float originalHealing, HealingFlags flags, UnitId? healer, AbilityKey? source)
        {
            Target = target;
            OriginalHealing = originalHealing;
            Flags = flags;
            Healer = healer;
            Source = source;
        }

        public UnitId Target { get; }
        public float OriginalHealing { get; }
        public HealingFlags Flags { get; }
        public UnitId? Healer { get; }
        public AbilityKey? Source { get; }
    }
}
