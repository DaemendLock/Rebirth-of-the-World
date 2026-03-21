using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.ValueObjects
{

    public readonly ref struct CastInfo
    {
        public readonly EntityId CasterId { get; }
        public readonly SkillId SkillId { get; }
    }
}
