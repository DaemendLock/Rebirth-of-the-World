using Combat.API;
using Combat.API.DTO;
using Combat.API.Statuses;
using Combat.Common.ValueObjects;
using Combat.Local.Data.Lookup;
using Combat.Local.Gateways.DataSources;

using System;
using System.Collections.Generic;

namespace Combat.Local.Data.Databases
{
    public class StatusModificationProvider : IStatusApiDataSource
    {
        private readonly StatusLookup _statusLookupService;

        private readonly Dictionary<EntityId, AttributeValue[]> _cachedAttributes;

        private readonly Stack<AttributeValue[]> _attributeArrayPool;

        public StatusModificationProvider(StatusLookup statusLookupService)
        {
            _statusLookupService = statusLookupService;

            //_cachedStates = new();
            _cachedAttributes = new();

            _attributeArrayPool = new();
        }

        public void ClearCache()
        {
            _statusLookupService.Update();
            ClearAttributesCache();
            //_cachedStates.Clear();
        }

        public bool RestrictMovement(EntityId entityId) => false;

        public AttributeValue[] GetAttributesModification(EntityId entityId, AttributeValue[] baseValues)
        {
            if (_cachedAttributes.TryGetValue(entityId, out AttributeValue[] bonuses))
            {
                return bonuses;
            }

            if (_attributeArrayPool.TryPop(out bonuses) == false)
            {
                bonuses = new AttributeValue[baseValues.Length];
            }

            _cachedAttributes[entityId] = bonuses;
            EvaluateAttributes(entityId, baseValues, bonuses);
            return bonuses;
        }

        private void ClearAttributesCache()
        {
            foreach (AttributeValue[] attributeValue in _cachedAttributes.Values)
            {
                _attributeArrayPool.Push(attributeValue);
            }

            _cachedAttributes.Clear();
        }

        private void EvaluateAttributes(EntityId entityId, ReadOnlySpan<AttributeValue> baseValues, AttributeValue[] buffer)
        {
            Array.Clear(buffer, 0, buffer.Length);
            AttributesData data = new(baseValues, buffer);

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
