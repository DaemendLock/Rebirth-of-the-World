using System;

using Utils.DataTypes;

namespace Data.Skills
{
    internal interface ISpellDataProvider : IDisposable
    {
        bool HasSpell(SpellId id);

        byte[] GetBytes(SpellId id);
    }
}
