using Combat.API;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.ValueObjects;

namespace TestSkillsPack.Paladin
{
    [SkillScriptName("TankPaladin3")]
    public class TankPaladin3 : SkillScript, ICastableSkill
    {
        private float _radius;
        private float _energyPerTarget;
        private float _spellPowerDamageRatio;

        protected override void OnInit()
        {
            _radius = 2f;
            _energyPerTarget = 10f;
            _spellPowerDamageRatio = 0.1f;
        }

        public void OnCast()
        {
            IUnit[] targets = Scene.FindUnitsInRadius(Owner.Position, _radius);
            int targetCount = 0;

            foreach (IUnit target in targets)
            {
                if (CanHit(target) == false)
                {
                    continue;
                }

                float damage = Owner.GetAttributeValue(Attribute.Spellpower) * _spellPowerDamageRatio;
                target.ApplyDamage(new(Owner, Instance, damage, Combat.Common.Flags.DamageFlags.None));
                targetCount++;
            }

            Scene.CreateStatus(new(Owner, "TankPaladin3Buff", 3f, 1, Instance));
            Owner.GiveResource(new(ResourceId.Custom, _energyPerTarget * targetCount, Instance));
        }

        private bool CanHit(IUnit target)
        {
            if (target.Team == Instance.Owner.Team)
            {
                return false;
            }

            return true;
        }
    }
}
