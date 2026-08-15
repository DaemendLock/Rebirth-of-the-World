using Combat.Common.Flags;
using Combat.Common.Primitives;

namespace Combat.API.Contexts
{
    public interface IAbilityContext
    {
        Unit Owner { get; }
        EncounterApi Scene { get; }
        SkillId SkillId { get; }

        bool HasFlag(SkillFlags skillFlags);
    }
}
