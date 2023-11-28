using System;

namespace Data.DataMapper
{
    internal class DataLoader<Key, Value> : IDisposable where Key : struct where Value : struct
    {
        private DataMap<Key, Value> _dataMap;

        private string _path;

        public DataLoader(string path)
        {
            _path = path;
        }

        public bool Loaded { get; private set; }

        public void Load()
        {
            if (Loaded)
            {
                return;
            }

            _dataMap = new DataMap<Key, Value>(_path);
            Loaded = true;
        }

        public void Clear()
        {
            if (Loaded == false)
            {
                return;
            }

            _dataMap?.Release();
            Loaded = false;
        }

        public void Reload()
        {
            Clear();
            Load();
        }

        public Key[] GetLoadedKeys() => _dataMap.GetKeys();

        public Value GetData(Key key) => _dataMap.GetData(key);

        public void Dispose()
        {
            _dataMap?.Dispose();
        }
    }
}
