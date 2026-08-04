using Combat.API.Scripting;
using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct DeathRecord
    {
        public DeathRecord(IUnit target, SkillScript source, KillFlags flags)
        {
            Target = target;
            Source = source;
            Flags = flags;
        }

        public IUnit Target { get; }
        public SkillScript Source { get; }
        public KillFlags Flags { get; }
    }
}
