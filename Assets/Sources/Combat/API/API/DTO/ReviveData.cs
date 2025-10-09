using Combat.Common.Flags;

namespace Combat.API.DTO
{
    public readonly ref struct ReviveData
    {
        public readonly Unit Reviver;
        public readonly SkillApi Source;
        public readonly ReviveFlags Flags;

        public ReviveData(Unit reviver, SkillApi source, ReviveFlags flags)
        {
            Reviver = reviver;
            Flags = flags;
            Source = source;
        }
    }
}
