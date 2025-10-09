using Combat.API;
using Combat.API.DTO;
using Combat.API.Skills;
using Combat.API.Utils;

namespace TestSkillPack.Assets.Skills.TestSkillsPack.Giantess
{
    [SkillScriptName("stomp_attack")]
    public class StompAttack : SkillScript
    {
        private ApplyDamageOptions _applyDamageOptions;

        protected override void OnInit()
        {
            _applyDamageOptions = new();
        }

        public bool OnHit(HitRecord @event)
        {
            //if (@event.Hurtbox.Owner is not IHurtboxOwner<Unit> hurtbox)
            //{
            //    return false;
            //}

            //Unit target = hurtbox.Owner;

            //if (Caster.CanHurt(target) == false)
            //{
            //    return false;
            //}

            //if ((Skill.Flags.HasFlag(SkillFlags.CanTargetDead) == false) && (target.Alive == false))
            //{
            //    return false;
            //}

            //TODO: CreateDamageEvent, get attacker bonuses, get defender bonuses, pass as DamageInstance

            //_applyDamageOptions.Target = target;
            //_applyDamageOptions.OriginalDamage = GetDamage(target);

            ApplyDamageOptionsExtension.ApplyDamage(_applyDamageOptions);

            return true;
        }

        private float GetDamage(Unit caster, Unit target)
        {
            float scaleDif = caster.Scale / target.Scale;

            if (scaleDif < 2)
            {
                return 0;
            }

            return 10 * scaleDif * scaleDif;
        }
    }
}
