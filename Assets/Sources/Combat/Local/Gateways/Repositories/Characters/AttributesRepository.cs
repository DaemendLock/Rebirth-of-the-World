using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public class AttributesRepository : IAttributesRepository
    {
        private readonly Dictionary<UnitId, AttributeData> _values;

        public AttributesRepository()
        {
            _values = new();
        }

        public void Create(AttributesOwner value)
        {
            float[] values = new float[value.BaseValues.Length];

            _values[value.Id] = new(value.BaseValues.ToArray(), values);
        }

        public AttributesOwner Get(UnitId id)
        {
            if (_values.TryGetValue(id, out AttributeData data) == false)
            {
                throw new System.ArgumentException();
            }

            return new(id, data.BaseValues, data.Values);
        }

        public void Update(AttributesOwner value)
        {
            if (_values.TryGetValue(value.Id, out AttributeData data) == false)
            {
                throw new System.ArgumentException();
            }

            value.BaseValues.CopyTo(data.BaseValues);

            if (value.GetAll().Length == 0)
            {
                System.Array.Clear(data.Values, 0, data.Values.Length);
                return;
            }

            value.GetAll().CopyTo(data.Values);
        }

        public void Delete(UnitId id)
        {
            _values.Remove(id);
        }
    }
}
