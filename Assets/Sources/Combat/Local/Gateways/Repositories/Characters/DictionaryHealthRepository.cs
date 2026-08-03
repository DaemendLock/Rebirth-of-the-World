using Combat.Common.ValueObjects;
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
        public const int MaxGenerationCount = 1024;

        private readonly Queue<int> _freeIndexes = new();
        private int _count = 0;

        private readonly Dictionary<UnitId, HealthValue> _values;

        private readonly HealthValue[] _valuesNew;
        private readonly Dictionary<UnitId, int> _indexes;

        private readonly IAttributesRepository _attributesRepository;

        public DictionaryHealthRepository(IAttributesRepository attributesRepository)
        {
            _attributesRepository = attributesRepository;

            _values = new();
            _valuesNew = new HealthValue[128];
            _indexes = new(128);
        }

        public Health Create(UnitId id, HealthValue health)
        {
            if (_freeIndexes.TryDequeue(out int index) == false)
            {
                index = _count;
            }

            if (index >= _valuesNew.Length)
            {
                throw new System.InvalidOperationException("Health component pool is full");
            }

            _indexes.Add(id, index);
            _count++;
            _valuesNew[index] = new();
            return new(id, new(index, _valuesNew));
        }

        //public void Create(Health value) => _values.Add(value.Id, new(value.CurrentHealth, value.DefaultHealth));

        public void Delete(UnitId id) => _values.Remove(id);

        public bool TryGet(UnitId id, out Health health)
        {
            AttributesOwner attributesOwner = _attributesRepository.Get(id);
            health = new(id, new(_indexes[id], _valuesNew), attributesOwner.GetMaxHealthBonus());
            return true;
        }

        public void Update(UnitId id, Health value) => _values[id] = new(value.CurrentHealth, value.DefaultHealth);
    }
}
