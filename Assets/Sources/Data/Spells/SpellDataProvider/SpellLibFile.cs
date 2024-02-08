using Data.DataMapper;
using Data.Spells;
using Data.Utils;
using Data.Utils.ThrowHepler;
using System;
using Utils.DataTypes;

namespace Data.Assets.Sources.Data.Spells
{
    internal class SpellLibFile : ISpellDataProvider
    {
        private long _datamapSize;
        private string _path;
        private File _file;
        private DataMap<SpellId, MappedData> _datamap;

        public SpellLibFile(string path)
        {
            _path = path;
            Load();
        }

        public byte[] GetBytes(SpellId id)
        {
            ThrowHepler.CheckFileLoad(_file);

            MappedData pointer = _datamap.GetData(id);

            _file.SetCursorPosition(pointer.Position + (_datamapSize * (sizeof(long) * 2 + sizeof(int))) + sizeof(long));
            return _file.ReadBytes(pointer.Size);
        }

        public bool HasSpell(SpellId id) => _datamap.HasKey(id);

        public void Dispose()
        {
            if (_file == null)
            {
                return;
            }

            _file.Dispose();
            _datamap.Release();
            _file = null;
        }

        private void Load()
        {
            if (_file != null)
            {
                return;
            }

            _file = new File(_path);
            _datamapSize = BitConverter.ToInt64(_file.ReadBytes(sizeof(long)));
            _datamap = new DataMap<SpellId, MappedData>(_file, _datamapSize);
        }
    }
}
