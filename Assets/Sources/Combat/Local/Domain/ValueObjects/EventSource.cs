using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.ValueObjects
{
    public readonly ref struct EventSource
    {
        public readonly UnitId? Unit;
        public readonly SkillId? Skill;

        public EventSource(UnitId? unit, SkillId? skill)
        {
            Unit = unit;
            Skill = skill;
        }
    }
}
