using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.ByteHelper;
using Utils.DataTypes;

namespace Core.Combat.Abilities.SpellEffects
{
    public class ReduceCooldown : SpellEffect
    {
        private readonly SpellId _spell;
        private readonly long _duration;

        public ReduceCooldown(Spell spell, long duration)
        {
            _spell = spell.Id;
            _duration = duration;
        }

        public ReduceCooldown(SpellId spellId, long duration)
        {
            _spell = spellId;
            _duration = duration;
        }

        public ReduceCooldown(ByteReader source)
        {
            _spell = (SpellId) source.ReadInt();
            _duration = source.ReadLong();
        }

        public ActionRecord ApplyEffect(EffectApplicationData data, float modification)
        {
            data.Target.FindAbility(Spell.Get(_spell)).ReduceCooldown((int) ((modification + _duration) * 1000));
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