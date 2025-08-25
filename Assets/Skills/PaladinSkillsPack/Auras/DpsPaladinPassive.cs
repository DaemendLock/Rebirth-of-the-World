using Combat.Local.Domain.API;
using Combat.Local.Domain.API.Statuses;
using Combat.Local.Domain.API.ValueObjects;

namespace TestSkillsPack.Paladin
{
    [StatusScriptName("DpsPaladinPassive")]
    public class DpsPaladinPassive : StatusApi, IOutgoingHealDamageModifier
    {
        public float GetBonusDamageDealthPercent(DamageInstance instance) => 0.1f * Parent.GetResourceValue(new(2));
    }
}
