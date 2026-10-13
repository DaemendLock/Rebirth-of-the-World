using System;
using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories
{
    public sealed class RefContainer<TKey, TValue> where TKey : notnull
    {
        private readonly Dictionary<TKey, int> _indexes;
        private readonly Stack<int> _freeIndexes;

        private TValue[] _values;
        private int _count;

        public RefContainer(int capacity = 4)
        {
            _indexes = new();
            _freeIndexes = new();
            _values = capacity > 0 ? new TValue[capacity] : Array.Empty<TValue>();
            _count = 0;
        }

        public int Count => _count;

        public int Capacity => _values.Length;

        public ref TValue this[TKey key]
        {
            get
            {
                if (_indexes.TryGetValue(key, out int index) == false)
                {
                    throw new KeyNotFoundException();
                }

                return ref _values[index];
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (_indexes.ContainsKey(key))
            {
                throw new System.InvalidOperationException("Key already present");
            }

            if (_freeIndexes.TryPop(out int index) == false)
            {
                if (_count == _values.Length)
                {
                    Array.Resize(ref _values, Math.Max(2, _values.Length * 3 / 2));
                }

                index = _count;
            }

            _count++;
            _values[index] = value;
            _indexes[key] = index;
            return;
        }

        public void Remove(TKey key)
        {
            if (_indexes.Remove(key, out int index) == false)
            {
                return;
            }

            _count--;
            _values[index] = default;
            _freeIndexes.Push(index);
        }

        public ref TValue Get(TKey key)
        {
            if (_indexes.TryGetValue(key, out int index) == false)
            {
                throw new KeyNotFoundException();
            }

            return ref _values[index];
        }
    }
}
