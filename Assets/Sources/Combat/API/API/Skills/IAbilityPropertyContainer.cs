using Combat.API.Skills;

namespace Combat.API
{
    public interface IAbilityPropertyContainer
    {
        void Give();
        void Remove();
        bool TryGet(out IHitHandler result);
        bool TryGet(out ICastStateChangeHandler result);
        bool TryGet(out ITargettableSkill result);
    }
}
