using Combat.Common.Primitives;

namespace Combat.Local.Domain.Endpoints.Skills
{
    public interface ISkillBehaviour
    {
        void Give();
        void Remove();
        bool TryGet(SkillId skillId, out ISkillExecutionPort result);
        bool TryGet(out ISkillHitHandler result);
        bool TryGet(out ISkillActionStateChangeHandler result);
        bool TryGet(out ILockTargetBehaviour result);
    }
}
