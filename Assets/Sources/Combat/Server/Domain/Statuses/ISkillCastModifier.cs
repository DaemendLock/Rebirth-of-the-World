using Server.Combat.Domain.Actions;

namespace Server.Combat.Domain.Statuses.StatusEffects
{
    public interface ISkillCastModifier
    {
        void OnStartCast(IActionHandler skillCastHandler);
        void OnEndCast(IActionHandler skillCastHandler);
    }
}
