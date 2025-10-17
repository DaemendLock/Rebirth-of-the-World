using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.OutputPorts
{
    public readonly ref struct SpendResourceResult
    {
        public SpendResourceResult(EntityId target, ResourceId resource, float value, SkillId? skill, EntityId? caster)
        {
            Target = target;
            Resource = resource;
            Value = value;
            Skill = skill;
            Caster = caster;
        }

        public EntityId Target { get; }
        public ResourceId Resource { get; }
        public float Value { get; }
        public SkillId? Skill { get; }
        public EntityId? Caster { get; }
    }

    public interface ISpendResourceEventHandler
    {
        void HandleEvent(SpendResourceResult result);
    }
}
