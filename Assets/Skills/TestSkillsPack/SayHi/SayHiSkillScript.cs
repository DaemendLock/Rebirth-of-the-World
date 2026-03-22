using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.API.Utils;
using Combat.Common.Flags;

namespace TestSkillsPack.SkillScripts
{
    [SkillScriptName("sayHi")]
    public class SayHiSkillScript : SkillScript, ICastStateChangeHandler, ICastableSkill, IHitHandler
    {
        protected override void OnInit()
        {
        }

        public void OnCast(CastEvent @event)
        {
            UnityEngine.Debug.Log("Hi~~~!");
            Scene.CreateStatus(new(@event.Caster, "HiStatus", 5, 1, Instance));

            var targets = Scene.FindUnitsInRadius(Instance.Owner.Position, 100f);

            foreach (var target in targets)
            {
                UnityEngine.Debug.Log($"Hi, {target.ModelName}[{target.Id}] of team {target.Team}");
            }
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

            ApplyDamageOptions applyDamageOptions = new()
            {
                Attacker = @event.Source,
                Target = @event.Target,
                Source = Instance,
                OriginalDamage = 5,
                Flags = DamageFlags.None,
            };

            applyDamageOptions.Target = @event.Target;
            applyDamageOptions.ApplyDamage();
            return true;
        }
    }
}