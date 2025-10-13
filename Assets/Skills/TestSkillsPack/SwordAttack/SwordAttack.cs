using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.API.Utils;

namespace Server.Combat.Domain.Implementations.Skills.SpellScripts
{
    [SkillScriptName("hitbox_1_attack")]
    public class SwordAttack : SkillScript
    {
        private ApplyDamageOptions _applyDamageOptions;

        protected override void OnInit()
        {
            _applyDamageOptions = new ApplyDamageOptions();
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
            _applyDamageOptions.OriginalDamage = GetDamage();

            ApplyDamageOptionsExtension.ApplyDamage(_applyDamageOptions);
            return true;
        }

        private float GetDamage() => 0;
    }
}