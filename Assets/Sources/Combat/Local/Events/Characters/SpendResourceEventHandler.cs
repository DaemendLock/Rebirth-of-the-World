using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Events
{
    public readonly ref struct SpendResourceInfo
    {
        public SpendResourceInfo(EntityId target, ResourceId resource, float value, SkillId? skill, EntityId? caster)
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

    public class SpendResourceEventHandler : ISpendResourceEventHandler
    {
        public delegate void Handler(SpendResourceInfo info);

        public event Handler Spent;

        public void HandleEvent(EntityId target, ResourceId resource, float value, EventSource source)
        {
            SpendResourceInfo info = new(target, resource, value, source.Skill, source.Unit);
            Spent?.Invoke(info);
        }
    }
}
