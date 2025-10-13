using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct KillInfo
    {
        public readonly Unit Attacker;
        public readonly SkillApi Source;
        public readonly KillFlags Flags;

        public KillInfo(Unit attacker, SkillApi source, KillFlags flags)
        {
            Attacker = attacker;
            Source = source;
            Flags = flags;
        }
    }
}
