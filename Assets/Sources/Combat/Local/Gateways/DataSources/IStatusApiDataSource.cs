using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Gateways.DataSources
{
    public interface IStatusApiDataSource
    {
        bool RestrictMovement(EntityId entityId);

        void GetAttributesModification(EntityId entityId, AttributeValue[] baseValues, Span<AttributeValue> target);
    }
}
