using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct DeathRecord
    {
        public DeathRecord(Unit target, SkillScript source, KillFlags flags)
        {
            Target = target;
            Source = source;
            Flags = flags;
        }

        public Unit Target { get; }
        public SkillScript Source { get; }
        public KillFlags Flags { get; }
    }
}
