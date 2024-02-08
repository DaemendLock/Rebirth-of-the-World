using Data.Spells;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Data.Assets.Sources.Data.Spells
{
    internal class MultiFileSpellDataProvider : ISpellDataProvider
    {
        private List<ISpellDataProvider> _spellLibFiles;

        public MultiFileSpellDataProvider(IEnumerable<ISpellDataProvider> sources)
        {
            _spellLibFiles = new(sources);
        }

        public byte[] GetBytes(SpellId id)
        {
            foreach (ISpellDataProvider source in _spellLibFiles)
            {
                if (source.HasSpell(id))
                {
                    return source.GetBytes(id);
                }
            }

            throw new KeyNotFoundException(id.ToString());
        }

        public bool HasSpell(SpellId id)
        {
            foreach (ISpellDataProvider source in _spellLibFiles)
            {
                if (source.HasSpell(id))
                {
                    return true;
                }
            }

            return false;
        }

        public void Dispose()
        {
            foreach (ISpellDataProvider source in _spellLibFiles)
            {
                source.Dispose();
            }
        }
    }
}
