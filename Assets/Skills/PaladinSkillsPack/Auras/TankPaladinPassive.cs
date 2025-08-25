using Combat.Local.Domain.API;
using Combat.Local.Domain.API.Statuses;
using Combat.Local.Domain.API.ValueObjects;

namespace TestSkillsPack.Paladin
{
    [StatusScriptName("TankPaladinPassive")]
    public class TankPaladinPassive : StatusApi, IIncomingHealDamageModifier
    {
        public float GetBonusDamageRecivedPercent(DamageInstance instance) => -0.1f * Parent.GetResourceValue(new(2));
    }
}
