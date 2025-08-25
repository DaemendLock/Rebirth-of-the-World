using Combat.Local.Domain.DTO;

namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct DeathRecord
    {
        public DeathRecord(Unit target, ScriptedSkill source, KillFlags flags)
        {
            Target = target;
            Source = source;
            Flags = flags;
        }

        public Unit Target { get; }
        public ScriptedSkill Source { get; }
        public KillFlags Flags { get; }
    }
}
