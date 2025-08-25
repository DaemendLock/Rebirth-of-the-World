using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.API.Statuses
{
    public interface IAttributesModifier : IStatusPropery
    {
        void ModifyAttributes(AttributesData data);
    }
}
