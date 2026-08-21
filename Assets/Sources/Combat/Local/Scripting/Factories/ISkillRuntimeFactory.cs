using Combat.Common.Primitives;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.Factories
{
    public interface ISkillRuntimeFactory
    {
        bool CanHandle(SkillId skillId);
        SkillRuntime Create(AbilityKey abilityKey);
    }
}
