using Server.Combat.Domain.Actions;
using Server.Combat.Domain.Implementations.Actions;
using Server.Combat.Domain.Skills;

namespace Server.Combat.Domain.Implementations.Utils.Extenstions
{
    public static class ActionHandlerExtension
    {
        //public static bool RestrictMovement(this IActionHandler actionHandler) => actionHandler is CastHandler cast && !cast.Skill.Flags.HasFlag(SkillFlags.DontRestrictMovement);
    }
}
