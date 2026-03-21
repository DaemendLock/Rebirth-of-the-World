using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Domain.Facades
{
    public readonly struct HealthOwnerFacade
    {
        private readonly SetHealthUseCase _setHealthUseCase;
        private readonly ApplyDamageUseCase _applyDamageUseCase;
        private readonly ApplyHealingUseCase _applyHealingUseCase;
        private readonly GetHealthUseCase _getHealthUseCase;

        public HealthOwnerFacade(SetHealthUseCase setHealthUseCase, ApplyDamageUseCase applyDamageUseCase, ApplyHealingUseCase applyHealingUseCase, GetHealthUseCase getHealthUseCase)
        {
            _setHealthUseCase = setHealthUseCase;
            _applyDamageUseCase = applyDamageUseCase;
            _applyHealingUseCase = applyHealingUseCase;
            _getHealthUseCase = getHealthUseCase;
        }

        public HealthValueDTO GetHealth(EntityId entityId)
        {
            Health value = _getHealthUseCase.Execute(entityId);
            return new(value.CurrentHealth, value.MaxHealth);
        }

        public void SetHealth(EntityId target, float value)
        {
            _setHealthUseCase.Execute(target, value);
        }

        public void ApplyDamage(ApplyDamageInfo info)
        {
            _applyDamageUseCase.Execute(info.Target, info.OriginalDamage, info.Flags, info.Attacker, new(info.Caster, info.Source));
        }

        public void ApplyHealing(ApplyHealingInfo info)
        {
            _applyHealingUseCase.Execute(info.Target, info.OriginalHealing, info.Flags, info.Healer, new(info.Caster, info.Source));
        }
    }

    public readonly ref struct HealthValueDTO
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
        public ApplyDamageInfo(EntityId target, float originalDamage, DamageFlags flags, EntityId? attacker, SkillId? source, EntityId? caster)
        {
            Target = target;
            OriginalDamage = originalDamage;
            Flags = flags;
            Attacker = attacker;
            Source = source;
            Caster = caster;
        }

        public EntityId Target { get; }
        public float OriginalDamage { get; }
        public DamageFlags Flags { get; }
        public EntityId? Attacker { get; }
        public SkillId? Source { get; }
        public EntityId? Caster { get; }
    }

    public readonly ref struct ApplyHealingInfo
    {
        public ApplyHealingInfo(EntityId target, float originalHealing, HealingFlags flags, EntityId? healer, SkillId? source, EntityId? caster)
        {
            Target = target;
            OriginalHealing = originalHealing;
            Flags = flags;
            Healer = healer;
            Source = source;
            Caster = caster;
        }

        public EntityId Target { get; }
        public float OriginalHealing { get; }
        public HealingFlags Flags { get; }
        public EntityId? Healer { get; }
        public SkillId? Source { get; }
        public EntityId? Caster { get; }
    }
}
