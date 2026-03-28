using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.OutputPorts
{
    public readonly ref struct SpendResourceResult
    {
        public SpendResourceResult(EntityId target, ResourceId resource, float value, float currentValue, float maxValue, SkillId? skill, EntityId? caster)
        {
            Target = target;
            Resource = resource;
            Value = value;
            Skill = skill;
            Caster = caster;
            CurrentValue = currentValue;
            MaxValue = maxValue;
        }

        public EntityId Target { get; }
        public ResourceId Resource { get; }
        public float Value { get; }
        public float CurrentValue { get; }
        public float MaxValue { get; }
        public SkillId? Skill { get; }
        public EntityId? Caster { get; }
    }

    public interface ISpendResourceOutput
    {
        void Present(Resource resource);
    }
}
