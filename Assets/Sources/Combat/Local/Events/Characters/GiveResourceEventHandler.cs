using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Events
{
    public readonly ref struct GiveResourceInfo
    {
        public GiveResourceInfo(EntityId target, ResourceId resourceId, float value, SkillId? skill, EntityId? caster)
        {
            Target = target;
            Resource = resourceId;
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

    public class GiveResourceEventHandler : IGiveResourceEventHandler
    {
        public delegate void Handler(GiveResourceInfo info);

        public event Handler Given;

        public void HandleEvent(EntityId target, ResourceId resource, float value, EventSource source)
        {
            GiveResourceInfo info = new(target, resource, value, source.Skill, source.Unit);
            Given?.Invoke(info);
        }
    }
}
