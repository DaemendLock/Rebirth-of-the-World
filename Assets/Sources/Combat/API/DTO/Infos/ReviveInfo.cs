using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct ReviveInfo
    {
        public readonly Unit Reviver;
        public readonly AbilityApi Source;
        public readonly ReviveFlags Flags;

        public ReviveInfo(Unit reviver, AbilityApi source, ReviveFlags flags)
        {
            Reviver = reviver;
            Flags = flags;
            Source = source;
        }
    }
}
