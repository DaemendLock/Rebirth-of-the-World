using Combat.Local.Domain.Endpoints.Skills;

namespace Combat.Local.Domain.Entities
{
    public interface IAbilityPropertyContainer
    {
        void Give();
        void Remove();
        bool TryGet(out ISkillHitHandler result);
        bool TryGet(out ISkillActionStateChangeHandler result);
        bool TryGet(out ILockTargetBehaviour result);
    }
}
