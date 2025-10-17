using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.DTO
{
    public readonly ref struct ApplStatusDTO
    {
        public ApplStatusDTO(EntityId parent, StatusName statusName, int initialStackCount, float initialDuration, SkillId? skill, EntityId? caster)
        {
            Parent = parent;
            StatusName = statusName;
            InitialStackCount = initialStackCount;
            InitialDuration = initialDuration;
            Skill = skill;
            Caster = caster;
        }

        public EntityId Parent { get; }
        public StatusName StatusName { get; }
        public int InitialStackCount { get; }
        public float InitialDuration { get; }

        public SkillId? Skill { get; }
        public EntityId? Caster { get; }
    }
}
