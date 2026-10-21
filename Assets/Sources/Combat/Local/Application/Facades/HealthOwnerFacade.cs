using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.UseCases;

namespace Combat.Local.Application.Facades
{
    public readonly struct HealthOwnerFacade
    {
        private readonly HealthSetUseCase _setHealthUseCase;
        private readonly HealthApplyDamageUseCase _applyDamageUseCase;
        private readonly HealthApplyHealingUseCase _applyHealingUseCase;

        private readonly IHealthOwnerRepository _healthRepository;

        public HealthOwnerFacade(HealthSetUseCase setHealthUseCase, HealthApplyDamageUseCase applyDamageUseCase, HealthApplyHealingUseCase applyHealingUseCase, IHealthOwnerRepository healthRepository)
        {
            _setHealthUseCase = setHealthUseCase;
            _applyDamageUseCase = applyDamageUseCase;
            _applyHealingUseCase = applyHealingUseCase;
            _healthRepository = healthRepository;
        }

        public HealthValueDTO GetHealth(UnitId entityId)
        {
            var value = _healthRepository.Get(entityId);
            return new(value.CurrentHealth, value.MaxHealth, value.CurrentComposure, value.MaxComposure);
        }

        public void SetHealth(UnitId target, float value)
        {
            _setHealthUseCase.Execute(target, value);
        }

        public void ApplyDamage(ApplyDamageInfo info)
        {
            _applyDamageUseCase.Execute(info);
        }

        public void SetComposure(UnitId target, float value)
        {
            _setHealthUseCase.SetComposure(target, value);
        }

        public void ApplyHealing(ApplyHealingInfo info)
        {
            _applyHealingUseCase.Execute(info.Target, info.OriginalHealing, info.Flags, info.Healer, info.Source);
        }
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
