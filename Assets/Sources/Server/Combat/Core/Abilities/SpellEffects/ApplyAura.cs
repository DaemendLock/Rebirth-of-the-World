using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.ByteHelper;
using Utils.DataTypes;

namespace Core.Combat.Abilities.SpellEffects
{
    public class ApplyAura : SpellEffect
    {
        private readonly SpellId _effectId;
        private readonly long _duratoin;

        public ApplyAura(SpellId effectId, long duration)
        {
            _effectId = effectId;
            _duratoin = duration;
        }

        public ApplyAura(ByteReader source)
        {
            _effectId = (SpellId) source.ReadInt();
            _duratoin = source.ReadLong();
        }

        public float GetValue(float modifyValue)
        {
            return (float) (1000 * modifyValue) + _duratoin;
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.APPLY_AURA);
            buffer.Write(_effectId);
            buffer.Write(_duratoin);
        }

        public ActionRecord ApplyEffect(EffectApplicationData data, float modification)
        {
            long duration = (long) (1000 * modification) + _duratoin;

            return new AuraApplicationRecord(data, _effectId, duration);
        }
    }
}