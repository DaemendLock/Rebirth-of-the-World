using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.DTO
{
    public readonly ref struct ApplStatusDTO
    {
        public ApplStatusDTO(EntityId target, StatusName statusName, float initialDuration, int initialStackCount, SkillId? skill, EntityId? caster)
        {
            Target = target;
            StatusName = statusName;
            InitialStackCount = initialStackCount;
            InitialDuration = initialDuration;
            Skill = skill;
            Caster = caster;
        }

        public EntityId Target { get; }
        public StatusName StatusName { get; }
        public float InitialDuration { get; }
        public int InitialStackCount { get; }

        public SkillId? Skill { get; }
        public EntityId? Caster { get; }
    }
}
