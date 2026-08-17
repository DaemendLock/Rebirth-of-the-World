using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

using System;
using System.Collections.Generic;

namespace Combat.Local.Gateways.DataSources
{
    public sealed class ComponentPool<T> where T : struct, IUnitComponent
    {
        private readonly Dictionary<UnitId, int> _indexes;
        private T[] _values;
        private int _count;

        public ComponentPool(int initialCapacity = 0)
        {
            _indexes = new(initialCapacity);
            _values = initialCapacity == 0 ? Array.Empty<T>() : new T[initialCapacity];
            _count = 0;
        }

        public void Add(T value)
        {
            if (_indexes.ContainsKey(value.Id))
            {
                throw new System.InvalidOperationException("Key already present");
            }

            if (_count == _values.Length)
            {
                int capacity = Math.Max(2, _values.Length * 3 / 2);
                Array.Resize(ref _values, capacity);
            }

            int index = _count++;
            _values[index] = value;
            _indexes[value.Id] = index;
        }

        public ref T Get(UnitId id) => ref _values[_indexes[id]];

        public Span<T> GetAll() => _values.AsSpan(0, _count);

        public void Remove(UnitId id)
        {
            if (_indexes.Remove(id, out int index) == false)
            {
                return;
            }

            int lastIndex = --_count;

            if (index != lastIndex)
            {
                T moved = _values[lastIndex];
                _values[index] = moved;
                _indexes[moved.Id] = index;
            }
        }
    }
}
