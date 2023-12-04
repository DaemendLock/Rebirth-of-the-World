using Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils.ThrowHepler.Exceptions;

namespace Data.DataMapper
{
    internal class DataMap<Key, Value> : IDisposable where Key : struct where Value : struct
    {
        private Dictionary<Key, Value> _map = new Dictionary<Key, Value>();

        private bool _disposed = false;

        public DataMap(string path)
        {
            using File source = new(path);

            while (source.HasNext)
            {
                _map.Add(source.ReadStruct<Key>(), source.ReadStruct<Value>());
            }
        }

        ~DataMap()
        {
            Dispose();
        }

        public Value GetData(Key key)
        {
            if (_disposed)
            {
                throw new FileNotLoadedException();
            }

            return _map[key];
        }

        public void Release()
        {
            _map.Clear();
            _disposed = true;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            Release();
            _disposed = true;
        }

        internal Key[] GetKeys() => _map.Keys.ToArray();

        internal bool HasKey(Key key)
        {
            return _map.ContainsKey(key);
        }
    }
}
