using Combat.API;
using Combat.API.DTO;
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
            Owner.ApplyHealing(new(Owner, Skill, 100, HealingFlags.CanRevive));
            UnityEngine.Debug.Log("Healing you!");
        }
    }
}
