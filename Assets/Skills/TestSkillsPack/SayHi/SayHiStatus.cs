using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Statuses;

namespace TestSkillPack.Assets.Skills.TestSkillsPack.SayHi
{
    [StatusScriptName("HiStatus")]
    public class SayHiStatus : StatusScript, IAttributesModifier, ITimeScaleModifier
    {
        public override void OnCreate()
        {
            Instance.StartPeriodicAction(0.5f / Source.Owner.GetHasteModifier());
        }

        public override void OnTick()
        {
        }

        public void GetAttributesBonuses(AttributesData data)
        {
            data[Combat.Common.ValueObjects.Attribute.Spellpower] = new(100f, 0f);
        }

        public float GetModification() => -50f;
    }
}
