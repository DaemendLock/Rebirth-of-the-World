using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.ValueObjects
{
    public readonly ref struct EventSource
    {
        public readonly EntityId? Unit;
        public readonly SkillId? Skill;

        public EventSource(EntityId? unit, SkillId? skill)
        {
            Unit = unit;
            Skill = skill;
        }
    }
}
