using Combat.API;
using Combat.API.Scripting;
using Combat.API.Skills;

namespace TestSkillsPack.Paladin
{
    [SkillScriptName("TankPaladin3")]
    public class TankPaladin3 : SkillScript, ICastableSkill
    {
        private float _radius;
        private float _spellPowerDamageRatio;

        protected override void OnInit()
        {
            _radius = 2f;
            _spellPowerDamageRatio = 0.1f;
        }

        public void OnCast(CastEvent @event)
        {
            float energy = @event.Caster.GetResourceValue(new(2));

            Unit[] targets = Scene.FindUnitsInRadius(@event.Caster.Position, _radius);

            foreach (Unit target in targets)
            {
                if (CanHit(target) == false)
                {
                    continue;
                }

                float damage = energy * @event.Caster.GetAttributeValue(Combat.Common.ValueObjects.Attribute.Spellpower) * _spellPowerDamageRatio;
                target.ApplyDamage(new(@event.Caster, Instance, damage, Combat.Common.Flags.DamageFlags.None));
            }

            @event.Scene.CreateStatus(new(@event.Caster, "TankPaladin3Buff", 3f, 1, @event.Skill));
            @event.Caster.SpendResource(new(2), energy, @event.Skill);
        }

        private bool CanHit(Unit target)
        {
            if (target.Team == Instance.Owner.Team)
            {
                return false;
            }

            return true;
        }
    }
}
