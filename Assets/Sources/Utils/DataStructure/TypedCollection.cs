using System.Collections;
using System.Collections.Generic;

namespace Utils.DataStructure
{
    public interface ITypedCollection
    {
        void Add(object value);
        bool TryAdd(object value);
        bool Remove(object value);
    }

    public class TypedCollection<T> : ITypedCollection, IEnumerable<T>
    {
        private readonly List<T> _values;

        public TypedCollection()
        {
            _values = new();
        }

        public void Add(object value)
        {
            if (value is not T typedValue)
            {
                return;
            }

            _values.Add(typedValue);
        }

        public bool TryAdd(object value)
        {
            if (value is not T typedValue)
            {
                return false;
            }

            _values.Add(typedValue);
            return true;
        }

        public bool Remove(object value)
        {
            if (value is not T typedValue)
            {
                return false;
            }

            return _values.Remove(typedValue);
        }

        public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
