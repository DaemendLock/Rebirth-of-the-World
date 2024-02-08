using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Abilities.SpellEffects
{
    public class ReduceCooldown : SpellEffect
    {
        private readonly SpellId _spell;
        private readonly float _duration;

        public ReduceCooldown(Spell spell, float duration)
        {
            _spell = spell.Id;
            _duration = duration;
        }

        public ReduceCooldown(SpellId spellId, float duration)
        {
            _spell = spellId;
            _duration = duration;
        }

        public ReduceCooldown(ByteReader source)
        {
            _spell = (SpellId) source.ReadInt();
            _duration = source.ReadFloat();
        }

        public ActionRecord ApplyEffect(Unit caster, Unit target, float modification)
        {
            throw new System.NotImplementedException();
        }

        public float GetValue(float modifyValue)
        {
            return _duration + modifyValue;
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.REDUCE_COOLDOWN);
            buffer.Write(_spell);
            buffer.Write(_duration);
        }
    }
}