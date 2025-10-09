using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct ReviveRecord
    {
        public ReviveRecord(Unit target, SkillScript source, ReviveFlags flags)
        {
            Target = target;
            Source = source;
            Flags = flags;
        }

        public Unit Target { get; }
        public SkillScript Source { get; }
        public ReviveFlags Flags { get; }
    }
}
