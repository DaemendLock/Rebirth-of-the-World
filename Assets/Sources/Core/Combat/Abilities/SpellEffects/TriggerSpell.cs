using Core.Combat.Utils;
using System.IO;
using Utils.DataTypes;

namespace Core.Combat.Abilities.SpellEffects
{
    public class TriggerSpell : SpellEffect
    {
        private readonly SpellId _spell;

        public TriggerSpell(SpellId spellId)
        {
            _spell = spellId;
        }

        public TriggerSpell(Spell spell)
        {
            _spell = spell.Id;
        }
        public TriggerSpell(BinaryReader source)
        {
            _spell = (SpellId) source.ReadInt32();
        }

        public void ApplyEffect(EventData data, float modifyValue)
        {
            data.Caster.CastSpell(new EventData(data.Caster, data.Target, SpellLibrary.SpellLib.GetSpell(_spell)));
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