using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Statuses;
using Combat.API.ValueObjects;

namespace TestSkillPack.Assets.Skills.TestSkillsPack.SayHi
{
    [StatusScriptName("HiStatus")]
    public class SayHiStatus : CustomStatusStrategy, IOutgoingDamageModifier, IAttributesModifier
    {
        public override void OnCreate()
        {
            Instance.StartPeriodicAction(0.5f / Source.Owner.GetHasteModifier());
        }

        public override void OnTick()
        {
        }

        public float GetDamageDealthModification_Bonus(DamageInstanceApi instance) => instance.Attacker.GetAttributeValue(Combat.Common.ValueObjects.Attribute.Spellpower);

        public void GetAttributesBonuses(AttributesData data)
        {
            data[Combat.Common.ValueObjects.Attribute.Spellpower] = new(100f, 0f);
        }
    }
}
