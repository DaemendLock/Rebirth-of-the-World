using Combat.Common.Flags;
using Combat.Common.ValueObjects;

namespace Combat.API
{
    public interface IAbilityApi
    {
        AbilityKey AbilityKey { get; }
        IUnit Owner { get; }
        IEncounterApi Scene { get; }
        SkillId SkillId { get; }

        bool HasFlag(SkillFlags skillFlags);
    }
}