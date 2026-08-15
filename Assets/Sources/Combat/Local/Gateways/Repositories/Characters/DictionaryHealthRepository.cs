using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories
{
    public sealed class GenerationContainer<T>
    {
        private readonly ICollection<T>[] _values;
        private int _cursor;

        public ICollection<T> Get(int generation) => _values[generation];

        public void Create()
        {
            _cursor = (_cursor + 1) % Capacity;
            ICollection<T> result = _values[_cursor];
            _values[_cursor].Clear();
        }

        public int Capacity => _values.Length;

        public int CurrentGeneration { get; }
    }

    public sealed class DictionaryHealthRepository : IHealthRepository
    {
        private readonly Dictionary<UnitId, HealthValue> _values;

        private readonly IAttributesRepository _attributesRepository;

        public DictionaryHealthRepository(IAttributesRepository attributesRepository)
        {
            _attributesRepository = attributesRepository;

            _values = new();
        }

        public void Create(HealthOwner value)
        {
            HealthValue data = new(value.CurrentValue, value.Default);
            _values.Add(value.Id, data);
        }

        //public void Create(Health value) => _values.Add(value.Id, new(value.CurrentHealth, value.DefaultHealth));

        public void Delete(UnitId id) => _values.Remove(id);

        public bool TryGet(UnitId id, out HealthOwner health)
        {
            if (_values.TryGetValue(id, out var data) == false)
            {
                health = default;
                return false;
            }

            AttributesOwner attributesOwner = _attributesRepository.Get(id);
            health = new(id, data.Current, data.Default, attributesOwner.GetMaxHealthBonus());
            return true;
        }

        public void Update(HealthOwner value) => _values[value.Id] = new(value.CurrentValue, value.Default);
    }
}
