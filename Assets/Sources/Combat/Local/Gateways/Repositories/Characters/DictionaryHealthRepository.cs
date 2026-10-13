using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System;
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
        private readonly Dictionary<UnitId, int> _indexes;
        private readonly Stack<int> _freeIndexes;
        private HealthOwner[] _values;
        private int _cursor;

        public DictionaryHealthRepository()
        {
            _indexes = new();
            _freeIndexes = new();
            _values = System.Array.Empty<HealthOwner>();
            _cursor = 0;
        }

        public void Create(HealthOwner value)
        {
            if (_indexes.ContainsKey(value.Id))
            {
                throw new System.InvalidOperationException();
            }

            if (_cursor == _values.Length)
            {
                Array.Resize(ref _values, Math.Max(2, _values.Length * 3 / 2));
            }

            if (_freeIndexes.TryPop(out int index) == false)
            {
                index = _cursor++;
            }

            _values[index] = value;
            _indexes[value.Id] = index;
        }

        public void Delete(UnitId id)
        {
            if (_indexes.Remove(id, out int index) == false)
            {
                return;
            }

            _freeIndexes.Push(index);
        }

        public ref HealthOwner Get(UnitId id)
        {
            if (_indexes.TryGetValue(id, out var index) == false)
            {
                throw new KeyNotFoundException($"Entity {id} not found.");
            }

            return ref _values[index];
        }
    }
}
