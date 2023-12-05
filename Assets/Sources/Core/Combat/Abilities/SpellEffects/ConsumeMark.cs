using Core.Combat.Utils;
using System.IO;
using Utils.DataTypes;
using Utils.Serializer;

namespace Core.Combat.Abilities.SpellEffects
{
    public class ConsumeMark
    {
        private readonly SpellId _markId;
        private readonly SpellEffect _triggerEffect;

        public ConsumeMark(SpellId markId, SpellEffect effect)
        {
            _markId = markId;
            _triggerEffect = effect;
        }

        public ConsumeMark(BinaryReader source)
        {
            _markId = (SpellId) source.ReadInt32();
            _triggerEffect = SpellSerializer.DeserilizeSpellEffect(source);
        }

        public float GetValue(float modifyValue)
        {
            return modifyValue + _triggerEffect.GetValue(modifyValue);
        }

        public void ApplyEffect(CastEventData data, float modifyValue)
        {
            if (data.Target.HasStatus(_markId) == false)
            {
                return;
            }

            _triggerEffect.ApplyEffect(data, modifyValue);
            data.Target.RemoveStatus(_markId);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.CONSUME_MARK);
            buffer.Write(_markId);
            _triggerEffect.Serialize(buffer);
        }
    }
}
