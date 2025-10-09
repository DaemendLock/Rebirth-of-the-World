using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct Skill
    {
        public SkillId Id { get; }
        public bool CanCast { get; }
    }
}
