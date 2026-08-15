using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Statuses;
using Combat.Common.ValueObjects;

namespace TestSkillPack.Assets.Skills.TestSkillsPack.SayHi
{
    [StatusScriptName("HiStatus")]
    public class SayHiStatus : StatusScript, IAttributesModifier
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
            data[UnitAttribute.Spellpower] = new(100f, 0f);
        }

        public float GetTimeModification() => -50f;
    }
}
