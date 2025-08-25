using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.DTO
{
    public readonly ref struct KillData
    {
        public readonly Unit Attacker;
        public readonly Skill Source;
        public readonly KillFlags Flags;

        public KillData(Unit attacker, Skill source, KillFlags flags)
        {
            Attacker = attacker;
            Source = source;
            Flags = flags;
        }
    }
}
