using Combat.API;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.Primitives;
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

        public bool OnCast()
        {
            Unit[] targets = Scene.FindUnitsInRadius(Owner.Position, _radius);
            int targetCount = 0;

            foreach (Unit target in targets)
            {
                if (CanHit(target) == false)
                {
                    continue;
                }

                float damage = Owner.GetAttributeValue(UnitAttribute.Spellpower) * _spellPowerDamageRatio;
                target.ApplyDamage(new(Owner, Instance, damage, Combat.Common.Flags.DamageFlags.None));
                targetCount++;
            }

            Scene.CreateStatus(new(Owner, "TankPaladin3Buff", 3f, 1, Instance));
            Owner.GiveResource(new(ResourceId.Custom, _energyPerTarget * targetCount, Instance));
            return true;
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
