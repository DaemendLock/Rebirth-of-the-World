using Combat.Local.Domain.API;
using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.Skills;

namespace Server.Combat.TestAbilityPack
{
    [SkillScriptName("healself")]
    public class LifegivingLight : ScriptedSkill, ICastStateChangeHandler
    {
        public LifegivingLight(ScriptedSkillContext context) : base()
        {
        }

        public void OnRecovery()
        {
            Caster.ApplyHealing(new(Caster, this, 100, HealingFlags.CanRevive));
            UnityEngine.Debug.Log("Healing you!");
        }
    }
}
