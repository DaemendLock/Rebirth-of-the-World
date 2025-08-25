using Combat.Local.Domain.DTO;

namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct ReviveRecord
    {
        public ReviveRecord(Unit target, ScriptedSkill source, ReviveFlags flags)
        {
            Target = target;
            Source = source;
            Flags = flags;
        }

        public Unit Target { get; }
        public ScriptedSkill Source { get; }
        public ReviveFlags Flags { get; }
    }
}
