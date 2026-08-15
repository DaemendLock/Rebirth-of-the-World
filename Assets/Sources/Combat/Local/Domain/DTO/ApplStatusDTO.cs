using Combat.Common.Primitives;

namespace Combat.Local.Domain.DTO
{
    public readonly ref struct ApplStatusDTO
    {
        public ApplStatusDTO(UnitId target, StatusType statusName, float initialDuration, int initialStackCount, AbilityKey? skill)
        {
            Target = target;
            StatusName = statusName;
            InitialStackCount = initialStackCount;
            InitialDuration = initialDuration;
            Ability = skill;
        }

        public UnitId Target { get; }
        public StatusType StatusName { get; }
        public float InitialDuration { get; }
        public int InitialStackCount { get; }

        public AbilityKey? Ability { get; }
    }
}
