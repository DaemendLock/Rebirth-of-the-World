using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities.Statuses
{
    public interface IModifyAttributesStrategy
    {
        AttributesModification ModifyAttributes();
    }
}
