using Combat.Local.Domain.DTO;

namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct ReviveData
    {
        public readonly Unit Reviver;
        public readonly ScriptedSkill Source;
        public readonly ReviveFlags Flags;

        public ReviveData(Unit reviver, ScriptedSkill source, ReviveFlags flags)
        {
            Reviver = reviver;
            Flags = flags;
            Source = source;
        }
    }
}
