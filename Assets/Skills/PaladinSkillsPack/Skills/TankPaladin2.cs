using Combat.API;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

namespace TestSkillsPack.Paladin
{
    [SkillScriptName("TankPaladin2")]
    public class TankPaladin2 : SkillScript, ICastableSkill, ICastStateChangeHandler, IPassiveSkill
    {
        private float _spellPowerHealRatio;

        public StatusType PassiveStatusName { get; } = new("TankPaladinPassive");

        protected override void OnInit()
        {
            _spellPowerHealRatio = 2f;
        }

        public void OnActive()
        {
            Unit owner = Instance.Owner;

            float energy = owner.GetResourceValue(new(2));
            float healing = energy * owner.GetAttributeValue(UnitAttribute.Spellpower) * _spellPowerHealRatio;

            owner.ApplyHealing(new(healing, Combat.Common.Flags.HealingFlags.None, Instance, owner));
            owner.SpendResource(new ResourceId(2), energy, Instance);
        }
    }
}
