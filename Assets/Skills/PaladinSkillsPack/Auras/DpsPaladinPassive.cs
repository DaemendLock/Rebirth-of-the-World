using Combat.API.Scripting;
using Combat.API.Statuses;
using Combat.API.ValueObjects;
using Combat.Common.ValueObjects;

namespace TestSkillsPack.Paladin
{
    [StatusScriptName("DpsPaladinPassive")]
    public class DpsPaladinPassive : StatusScript, IOutgoingHealDamageModifier
    {
        public float GetBonusDamageDealthPercent(DamageInstanceApi instance) => 0.1f * Parent.GetResourceValue(ResourceId.Custom);
    }
}
