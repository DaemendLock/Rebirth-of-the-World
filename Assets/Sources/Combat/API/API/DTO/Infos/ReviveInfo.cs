using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct ReviveInfo
    {
        public readonly IUnit Reviver;
        public readonly IAbilityApi Source;
        public readonly ReviveFlags Flags;

        public ReviveInfo(IUnit reviver, IAbilityApi source, ReviveFlags flags)
        {
            Reviver = reviver;
            Flags = flags;
            Source = source;
        }
    }
}
