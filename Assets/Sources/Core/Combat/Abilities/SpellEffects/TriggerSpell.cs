using Core.Combat.Utils;
using System.IO;
using Utils.DataTypes;
using Utils.Serializer;

namespace Core.Combat.Abilities.SpellEffects
{
    public class TriggerSpell : SpellEffect
    {
        private readonly SpellId _spell;

        public TriggerSpell(SpellId spellId)
        {
            _spell = spellId;
        }

        public TriggerSpell(BinaryReader source)
        {
            _spell = (SpellId) source.ReadInt32();
        }

        public void ApplyEffect(CastEventData data, float modifyValue)
        {
            data.Caster.CastSpell(new CastEventData(data.Caster, data.Target, Spell.Get(_spell)));
        }

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