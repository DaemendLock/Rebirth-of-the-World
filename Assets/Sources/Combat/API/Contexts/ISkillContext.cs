using Combat.API.Skills;
using Combat.Common.Primitives;

namespace Combat.API.Contexts
{
    public interface ISkillContext
    {
        AbilityKey Key { get; }

        void StartCooldown(float value);

        SkillState<T> GetState<T>() where T : unmanaged, IDynamicSkillData;
        void SaveState<T>(SkillState<T> value) where T : unmanaged, IDynamicSkillData;

        TQuery GetCapability<TQuery>() where TQuery : class;
    }
}
