using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Statuses;
using Combat.Common.ValueObjects;

namespace TestSkillsPack.Paladin
{
    [StatusScriptName("TankPaladin3Buff")]
    public class TankPaladin3Buff : CustomStatusStrategy, IAttributesModifier
    {
        public void GetAttributesBonuses(AttributesData data)
        {
            data[Attribute.Speed] += new AttributeValue(0, 20f, 0);
        }
    }
}
