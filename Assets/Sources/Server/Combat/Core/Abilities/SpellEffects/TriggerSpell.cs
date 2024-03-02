using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.ByteHelper;
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

        public TriggerSpell(ByteReader source)
        {
            _spell = (SpellId) source.ReadInt();
        }

        public ActionRecord ApplyEffect(EffectApplicationData data, float modififaction) => Spell.Get(_spell).Cast(data.Caster, data.Target, data.Caster.GetSpellValues(_spell));

        public float GetValue(float modifyValue)
        {
            return _spell;
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.TRIGGER_SPELL);
            buffer.Write(_spell);
        }
    }
}