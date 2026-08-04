using Combat.API.Scripting;
using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct ReviveRecord
    {
        public ReviveRecord(IUnit target, SkillScript source, ReviveFlags flags)
        {
            Target = target;
            Source = source;
            Flags = flags;
        }

        public IUnit Target { get; }
        public SkillScript Source { get; }
        public ReviveFlags Flags { get; }
    }
}
