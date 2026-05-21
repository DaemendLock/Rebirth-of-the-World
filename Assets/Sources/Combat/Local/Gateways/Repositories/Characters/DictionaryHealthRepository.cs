using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories
{
    public sealed class DictionaryHealthRepository : IHealthRepository
    {
        private readonly Queue<int> _freeIndexes = new();
        private int _count = 0;

        private readonly Dictionary<UnitId, HealthData> _values;

        private readonly Dictionary<UnitId, int> _indexes = new();

        private readonly HealthValue[] _valuesNew;

        private readonly IAttributesRepository _attributesRepository;

        public DictionaryHealthRepository(IAttributesRepository attributesRepository)
        {
            _attributesRepository = attributesRepository;

            _values = new();
            _valuesNew = new HealthValue[128];
        }

        public Health Create(UnitId id)
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

        public Health Get(UnitId id)
        {
            AttributesOwner attributesOwner = _attributesRepository.Get(id);
            Health health = new(id, new(_indexes[id], _valuesNew), attributesOwner.GetMaxHealthBonus());
            return health;
        }

        public void Update(Health value) => _values[value.Id] = new(value.CurrentHealth, value.DefaultHealth);
    }
}
