using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Domain.Facades
{
    public readonly struct HealthOwnerFacade
    {
        private readonly HealthSetUseCase _setHealthUseCase;
        private readonly HealthApplyDamageUseCase _applyDamageUseCase;
        private readonly HealthApplyHealingUseCase _applyHealingUseCase;

        private readonly IHealthRepository _healthRepository;

        public HealthOwnerFacade(HealthSetUseCase setHealthUseCase, HealthApplyDamageUseCase applyDamageUseCase, HealthApplyHealingUseCase applyHealingUseCase, IHealthRepository healthRepository)
        {
            _setHealthUseCase = setHealthUseCase;
            _applyDamageUseCase = applyDamageUseCase;
            _applyHealingUseCase = applyHealingUseCase;
            _healthRepository = healthRepository;
        }

        public HealthValueDTO GetHealth(UnitId entityId)
        {
            if (_healthRepository.TryGet(entityId, out Health value) == false)
            {
                throw new System.InvalidOperationException();
            }

            return new(value.CurrentHealth, value.MaxHealth);
        }

        public void SetHealth(UnitId target, float value)
        {
            _setHealthUseCase.Execute(target, value);
        }

        public void ApplyDamage(ApplyDamageInfo info)
        {
            _applyDamageUseCase.Execute(info.Target, info.OriginalDamage, info.Flags, info.Attacker, info.Source);
        }

        public void ApplyHealing(ApplyHealingInfo info)
        {
            _applyHealingUseCase.Execute(info.Target, info.OriginalHealing, info.Flags, info.Healer, info.Source);
        }
    }

    public readonly struct HealthValueDTO
    {
        public HealthValueDTO(float currentHealth, float maxHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
        }

        public float CurrentHealth { get; }
        public float MaxHealth { get; }
    }

    public readonly ref struct ApplyDamageInfo
    {
        public ApplyDamageInfo(UnitId target, float originalDamage, DamageFlags flags, UnitId? attacker, AbilityKey? source)
        {
            Target = target;
            OriginalDamage = originalDamage;
            Flags = flags;
            Attacker = attacker;
            Source = source;
        }

        public UnitId Target { get; }
        public float OriginalDamage { get; }
        public DamageFlags Flags { get; }
        public UnitId? Attacker { get; }
        public AbilityKey? Source { get; }
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
