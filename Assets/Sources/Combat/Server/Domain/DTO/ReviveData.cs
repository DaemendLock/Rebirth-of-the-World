using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.DTO
{
    public readonly ref struct ReviveData
    {
        public readonly Unit Reviver;
        public readonly Skill Source;
        public readonly ReviveFlags Flags;

        public ReviveData(Unit reviver, Skill source, ReviveFlags flags)
        {
            Reviver = reviver;
            Flags = flags;
            Source = source;
        }
    }
}
