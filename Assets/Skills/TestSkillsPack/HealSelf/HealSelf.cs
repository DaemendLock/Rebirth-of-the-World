using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.Flags;

namespace Server.Combat.TestAbilityPack
{
    [SkillScriptName("healself")]
    public class LifegivingLight : SkillScript, ICastStateChangeHandler
    {
        public LifegivingLight(ScriptedSkillContext context) : base()
        {
        }

        public void OnRecovery()
        {
            Owner.ApplyHealing(new(100, HealingFlags.CanRevive, Skill, Owner));
            UnityEngine.Debug.Log("Healing you!");
        }
    }
}
