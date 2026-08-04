using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct KillInfo
    {
        public readonly Unit Attacker;
        public readonly AbilityApi Source;
        public readonly KillFlags Flags;

        public KillInfo(Unit attacker, AbilityApi source, KillFlags flags)
        {
            Attacker = attacker;
            Source = source;
            Flags = flags;
        }
    }
}
