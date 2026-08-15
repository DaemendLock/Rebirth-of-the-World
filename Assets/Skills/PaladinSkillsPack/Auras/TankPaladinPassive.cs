using Combat.API.Scripting;
using Combat.API.Statuses;
using Combat.API.ValueObjects;

namespace TestSkillsPack.Paladin
{
    [StatusScriptName("TankPaladinPassive")]
    public class TankPaladinPassive : StatusScript, IIncomingHealDamageModifier
    {
        public float GetBonusDamageRecivedPercent(DamageInstanceApi instance) => -0.1f * Parent.GetResourceValue(new(2));
    }
}
