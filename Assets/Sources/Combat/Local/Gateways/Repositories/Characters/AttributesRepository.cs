using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

using System;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public class AttributesRepository : IAttributesRepository
    {
        private readonly ComponentPool<AttributesOwner> _values;

        public AttributesRepository()
        {
            _values = new();
        }

        public void Create(AttributesOwner value) => _values.Add(value);

        public ref AttributesOwner Get(UnitId id) => ref _values.Get(id);

        public Span<AttributesOwner> GetAll() => _values.GetAll();

        public void Delete(UnitId id) => _values.Remove(id);
    }
}
