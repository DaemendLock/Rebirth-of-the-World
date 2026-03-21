using Combat.API.Scripting;
using Combat.API.Skills;

namespace Server.Combat.TestAbilityPack
{
    [SkillScriptName("healself")]
    public class LifegivingLight : SkillScript, ICastStateChangeHandler
    {
        public void OnRecovery()
        {
            //Owner.ApplyHealing(new(100, HealingFlags.CanRevive, @event.Skill, Owner));
            UnityEngine.Debug.Log("Healing you!");
        }
    }
}
