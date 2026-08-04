using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct KillInfo
    {
        public readonly IUnit Attacker;
        public readonly IAbilityApi Source;
        public readonly KillFlags Flags;

        public KillInfo(IUnit attacker, IAbilityApi source, KillFlags flags)
        {
            Attacker = attacker;
            Source = source;
            Flags = flags;
        }
    }
}
