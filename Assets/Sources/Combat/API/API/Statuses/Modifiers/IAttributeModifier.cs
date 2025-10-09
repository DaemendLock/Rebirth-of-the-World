using Combat.API.DTO;

namespace Combat.API.Statuses
{
    public interface IAttributesModifier : IStatusPropery
    {
        void GetAttributesBonuses(AttributesData data);
    }
}
