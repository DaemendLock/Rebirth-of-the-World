using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

using System;
using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Unit
{
    public class AttributesRepository : IAttributesRepository
    {
        private readonly IStatusApiDataSource _statusModificationProvider;
        private readonly Stack<AttributeValue[]> _objectPool;
        private readonly Dictionary<EntityId, AttributeValue[]> _values;
        private readonly Dictionary<EntityId, AttributeValue[]> _cachedBonuses;

        public AttributesRepository(IStatusApiDataSource apiDataSource)
        {
            _values = new();
            _cachedBonuses = new();
            _objectPool = new();

            _statusModificationProvider = apiDataSource;
        }

        public void Create(Attributes value)
        {
            _values.Add(value.Id, value.GetAllBase().ToArray());
        }

        public Attributes Get(EntityId id)
        {
            AttributeValue[] baseValues = _values[id];

            if (_cachedBonuses.TryGetValue(id, out AttributeValue[] bonusValues) == false)
            {
                if (_objectPool.TryPop(out bonusValues) == false)
                {
                    bonusValues = new AttributeValue[baseValues.Length];
                }

                _cachedBonuses[id] = bonusValues;

                Span<AttributeValue> buffer = stackalloc AttributeValue[baseValues.Length];
                _statusModificationProvider.GetAttributesModification(id, baseValues, buffer);
                buffer.CopyTo(bonusValues);
            }

            return new(id, baseValues, bonusValues);
        }

        public void Update(Attributes value)
        {
            _values[value.Id] = value.GetAllBase().ToArray();
        }

        public void Delete(EntityId id)
        {
            _values.Remove(id);
        }

        public void ClearCache()
        {
            foreach (AttributeValue[] value in _cachedBonuses.Values)
            {
                Array.Clear(value, 0, value.Length);
                _objectPool.Push(value);
            }

            _cachedBonuses.Clear();
        }

        public IReadOnlyCollection<EntityId> GetAllIds() => _values.Keys;
    }
}
