using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.DataStructure;
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

        public ActionRecord ApplyEffect(Unit caster, Unit target, float modififaction) => Spell.Get(_spell).Cast(caster, target, caster.GetSpellValues(_spell));

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