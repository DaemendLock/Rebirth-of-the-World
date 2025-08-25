using Server.Combat.Domain.Implementations.Actions;
using Server.Combat.Domain.Skills;

namespace Server.Combat.Domain.Implementations.Utils.Extenstions
{
    public static class AbilityExtension
    {
        //public static void EndCooldown(this CastHandler ability) => ability.Actor.SetCooldown(ability.Skill.Id, new(0));

        //public static void StartCooldown(this CastHandler ability)
        //{
        //    float cooldown = ability.Skill.Cooldown;

        //    if (ability.Skill.Flags.HasFlag(SkillFlags.HasteDontAffectsCooldown) == false)
        //    {
        //        cooldown /= ability.Actor.EvaluateHasteMultiplier();
        //    }

        //    ability.Actor.SetCooldown(ability.Skill.Id, new(cooldown));
        //}
    }
}
