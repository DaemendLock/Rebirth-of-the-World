using Core.Combat.Statuses.AuraEffects;
using Core.Combat.Utils;
using System.IO;
using Utils.Serializer;

namespace Core.Combat.Abilities.SpellEffects
{
    public class ApplyAura : SpellEffect
    {
        private readonly AuraEffect _effect;
        private readonly int _value;

        public ApplyAura(AuraEffect effect, int value = 0)
        {
            _effect = effect;
            _value = value;
        }

        public ApplyAura(BinaryReader source)
        {
            _effect = SpellSerializer.DeserializeAuraEffect(source);
            _value = source.ReadInt32();
        }

        public float GetValue(float modifyValue)
        {
            return modifyValue + _value;
        }

        public void ApplyEffect(CastEventData data, float modifyValue)
        {
            if (modifyValue + _value < 0)
            {
                return;
            }

            data.Target.ApplyAura(data, _effect);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.APPLY_AURA);
            _effect.Serialize(buffer);
            buffer.Write(_value);
        }
    }
}