using Combat.Local.Scripting.Idk.Capabilities.Skills;

namespace Combat.Local.Scripting.IDK
{
    public interface IAbilityPropertyContainer
    {
        bool TryGet(out ExecuteSkillCapability capability);
        bool TryGet(out HandleSkillHitCapability capability);
        bool TryGet(out HandleSkillActionStateChangeCapability capability);
        bool TryGet(out EvaluateSkillTargetCapability capability);
    }
}
