using Combat.Common.ValueObjects;
using Combat.Local.Data.Models;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Unit
{
    public class AttributesRepository : IAttributesRepository
    {
        private readonly Dictionary<EntityId, AttributeData> _values;

        public AttributesRepository()
        {
            _values = new();
        }

        public void Create(AttributesOwner value)
        {
            float[] values = new float[value.GetAllBase().Length];

            _values[value.Id] = new(value.GetAllBase().ToArray(), values);
        }

        public AttributesOwner Get(EntityId id)
        {
            if (_values.TryGetValue(id, out var data) == false)
            {
                throw new System.ArgumentException();
            }

            return new(id, data.BaseValues, data.Values);
        }

        public void Update(AttributesOwner value)
        {
            if (_values.TryGetValue(value.Id, out var data) == false)
            {
                throw new System.ArgumentException();
            }

            value.GetAllBase().CopyTo(data.BaseValues);
            value.GetAll().CopyTo(data.Values);
        }

        public void Delete(EntityId id)
        {
            _values.Remove(id);
        }
    }
}
