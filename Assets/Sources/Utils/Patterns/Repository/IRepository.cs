using System.Collections;
using System.Collections.Generic;

namespace Utils.Patterns.Repository
{
    public interface IRepository<TValue, TKey>
    {
        TValue Get(TKey key);
        void Add(TKey key, TValue value);
        bool Remove(TKey key);

        sealed TValue this[TKey key]
        {
            get => Get(key);
        }
    }

    public class DictionaryRepository<TValue, TKey> : IRepository<TValue, TKey>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        protected readonly Dictionary<TKey, TValue> Values;

        public DictionaryRepository()
        {
            Values = new();
        }

        public void Add(TKey key, TValue value) => Values[key] = value;
        public TValue Get(TKey key) => Values.GetValueOrDefault(key, default);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Values.GetEnumerator();
        public bool Remove(TKey key) => Values.Remove(key);
        IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();
    }
}
