using Core.Combat.Utils;
using Core.Data.SpellLib;
using System.IO;

namespace Core.Combat.Abilities.SpellEffects
{
    public class TriggerSpell : SpellEffect
    {
        private readonly int _spell;

        public TriggerSpell(Spell spell)
        {
            _spell = spell.Id;
        }

        public TriggerSpell(int spellId)
        {
            _spell = spellId;
        }
        public TriggerSpell(BinaryReader source)
        {
            _spell = source.ReadInt32();
        }

        public void ApplyEffect(EventData data, float modifyValue)
        {
            data.Caster.CastSpell(new EventData(data.Caster, data.Target, Data.SpellLib.SpellLib.GetSpell(_spell)));
        }

        public float GetValue(float modifyValue)
        {
            return _spell;
        }
        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            buffer.Write(_spell);
        }
    }
}