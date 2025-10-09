using Combat.Common.ValueObjects;

namespace Combat.Local.Gateways.DataSources
{
    public interface IStatusApiDataSource
    {
        bool RestrictMovement(EntityId entityId);

        AttributeValue[] GetAttributesModification(EntityId entityId, AttributeValue[] baseValues);
    }
}
