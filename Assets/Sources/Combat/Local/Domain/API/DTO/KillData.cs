using Combat.Local.Domain.DTO;

namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct KillData
    {
        public readonly Unit Attacker;
        public readonly ScriptedSkill Source;
        public readonly KillFlags Flags;

        public KillData(Unit attacker, ScriptedSkill source, KillFlags flags)
        {
            Attacker = attacker;
            Source = source;
            Flags = flags;
        }
    }
}
