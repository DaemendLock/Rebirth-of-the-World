using Combat.Local.Domain.Entities.Skills.Effects;

namespace Combat.Local.Domain.Entities
{
    public interface IAbilityPropertyContainer
    {
        void Give();
        void Remove();
        bool TryGet(out SkillCastEffect result);
        bool TryGet(out ISkillHitStrategy result);
        bool TryGet(out SkillActionStateChangeEffect result);
        bool TryGet(out LockTargetSkillEffect result);
    }
}
