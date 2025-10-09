using Combat.API;
using Combat.API.DTO;
using Combat.API.Skills;
using Combat.API.Utils;
using Combat.Common.Flags;

namespace TestSkillsPack.SkillScripts
{
    [SkillScriptName("sayHi")]
    public class SayHiSkillScript : SkillScript, ICastStateChangeHandler, ICastableSkill, IHitHandler
    {
        private ApplyDamageOptions _applyDamageOptions;

        protected override void OnInit()
        {
            _applyDamageOptions = new()
            {
                Attacker = Owner,
                Target = Owner,
                Source = Skill,
                OriginalDamage = 5,
                Flags = DamageFlags.None,
            };
        }

        public void OnCast()
        {
            UnityEngine.Debug.Log("Hi~~~!");
            Owner.ApplyStatus(new("HiStatus", Skill, 5, 1));
        }

        public void OnStartup()
        {
            UnityEngine.Debug.Log("Mei-san!");
        }

        public void OnEnds()
        {
            UnityEngine.Debug.Log("Kiana-chan!");
        }

        public bool OnHit(HitRecord @event)
        {
            if (@event.Target == @event.Source) { return false; }

            UnityEngine.Debug.Log($"Handling hit;");
            _applyDamageOptions.Target = @event.Target;
            _applyDamageOptions.ApplyDamage();
            return true;
        }
    }
}