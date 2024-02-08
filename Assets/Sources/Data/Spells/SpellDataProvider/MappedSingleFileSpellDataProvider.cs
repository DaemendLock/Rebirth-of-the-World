using Data.DataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils.DataTypes;

namespace Data.Spells
{
    internal class MappedSingleFileSpellDataProvider : ISpellDataProvider, IDisposable
    {
        private MappedDataLoader _combat;
        private DataLoader<SpellId, MappedData> _spellMap;

        public MappedSingleFileSpellDataProvider(string dataMapPath, string sourcePath)
        {
            _spellMap = new(dataMapPath);
            _combat = new(sourcePath);

            Load();
        }

        ~MappedSingleFileSpellDataProvider()
        {
            Dispose();
        }

        public bool Disposed { get; private set; } = true;

        public void Load()
        {
            if (Disposed == false)
            {
                return;
            }

            _spellMap.Load();
            _combat.Load();

            Disposed = false;
        }

        public byte[] GetBytes(SpellId id)
        {
            if (Disposed)
            {
                throw new InvalidOperationException("Can't read from unloaded data");
            }

            if (HasSpell(id) == false)
            {
                throw new KeyNotFoundException(id.ToString());
            }

            return _combat.GetBytes(_spellMap.GetData(id));
        }

        public bool HasSpell(SpellId id)
        {
            if (Disposed)
            {
                throw new InvalidOperationException("Can't read from unloaded data");
            }

            return _spellMap.GetLoadedKeys().Contains(id);
        }

        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            _combat.Dispose();
            _spellMap.Dispose();
            Disposed = true;
        }
    }
}
