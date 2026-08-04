using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Skills.Effects;

namespace Combat.Local.Domain.Endpoints.Skills
{

    public interface ISkillBehaviour
    {
        void Give();
        void Remove();
        bool TryGet(SkillId skillId, out SkillCastEffect result);
        bool TryGet(out ISkillHitHandler result);
        bool TryGet(out SkillActionStateChangeEffect result);
        bool TryGet(out LockTargetSkillEffect result);
    }
}
