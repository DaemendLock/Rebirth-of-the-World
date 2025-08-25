using Combat.Local.Domain.API;
using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.Skills;

namespace TestSkillsPack.SkillScripts
{
    [SkillScriptName("sayHi")]
    public class SayHiSkillScript : ScriptedSkill, ICastStateChangeHandler
    {
        private ApplyDamageOptions _applyDamageOptions;

        protected override void OnInit()
        {
            _applyDamageOptions = new()
            {
                Attacker = Caster,
                Target = Caster,
                Source = this,
                OriginalDamage = 5,
                Flags = DamageFlags.None,
            };
        }

        public override void OnCast()
        {
            UnityEngine.Debug.Log("Hi~~~!");
            Caster.ApplyStatus(new("HiStatus", this, 5, 1));
        }

        public void OnStartup()
        {
            UnityEngine.Debug.Log("Mei-san!");
        }

        public void OnEnds()
        {
            _applyDamageOptions.ApplyDamage();
            UnityEngine.Debug.Log("Kiana-chan!");
        }
    }
}