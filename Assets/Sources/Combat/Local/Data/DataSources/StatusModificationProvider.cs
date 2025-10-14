using Combat.API;
using Combat.API.DTO;
using Combat.API.Statuses;
using Combat.Common.ValueObjects;
using Combat.Local.Data.Lookup;
using Combat.Local.Gateways.DataSources;

using System;

namespace Combat.Local.Data.Databases
{
    public class StatusModificationProvider : IStatusApiDataSource
    {
        private readonly StatusLookup _statusLookupService;

        public StatusModificationProvider(StatusLookup statusLookupService)
        {
            _statusLookupService = statusLookupService;
        }

        public void ClearCache()
        {
            _statusLookupService.Update();
        }

        public bool RestrictMovement(EntityId entityId) => false;

        public void GetAttributesModification(EntityId entityId, AttributeValue[] baseValues, Span<AttributeValue> target)
        {
            AttributesData data = new(baseValues, target);

            foreach (StatusApi status in _statusLookupService.FindStatusesOnUnit(entityId))
            {
                if (status.TryGetProperty(out IAttributesModifier modifier) == false)
                {
                    continue;
                }

                modifier.GetAttributesBonuses(data);
            }
        }
    }
}
