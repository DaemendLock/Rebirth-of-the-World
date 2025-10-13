using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Unit
{
    public class AttributesRepository : IAttributesRepository
    {
        private readonly IStatusApiDataSource _statusModificationProvider;
        private readonly Dictionary<EntityId, AttributeValue[]> _values;

        public AttributesRepository(IStatusApiDataSource apiDataSource)
        {
            _values = new();
            _statusModificationProvider = apiDataSource;
        }

        public void Create(Attributes value)
        {
            _values.Add(value.Id, value.GetAllBase().ToArray());
        }

        public Attributes Get(EntityId id)
        {
            AttributeValue[] baseValues = _values[id];
            AttributeValue[] bonusValues = _statusModificationProvider.GetAttributesModification(id, baseValues);
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

        public IReadOnlyCollection<EntityId> GetAllIds() => _values.Keys;
    }
}
