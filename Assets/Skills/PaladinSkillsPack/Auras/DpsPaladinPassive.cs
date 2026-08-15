using Combat.API.Scripting;
using Combat.API.Statuses;
using Combat.API.ValueObjects;
using Combat.Common.Primitives;

namespace TestSkillsPack.Paladin
{
    [StatusScriptName("DpsPaladinPassive")]
    public class DpsPaladinPassive : StatusScript, IOutgoingDamageModifier
    {
        public float GetDamageDealthModification_Percent(DamageInstanceApi instance) => 0.1f * Parent.GetResourceValue(ResourceId.Custom);
    }
}
