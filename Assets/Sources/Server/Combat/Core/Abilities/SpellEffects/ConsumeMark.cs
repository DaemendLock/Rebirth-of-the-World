using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.ByteHelper;
using Utils.DataTypes;

namespace Core.Combat.Abilities.SpellEffects
{
    public class ConsumeMark : SpellEffect
    {
        private readonly SpellId _markId;
        private readonly SpellEffect _triggerEffect;

        public ConsumeMark(SpellId markId, SpellEffect effect)
        {
            _markId = markId;
            _triggerEffect = effect;
        }

        public ConsumeMark(ByteReader source)
        {
            _markId = (SpellId) source.ReadInt();
            _triggerEffect = SpellSerializer.DeserilizeSpellEffect(source);
        }

        public float GetValue(float modifyValue)
        {
            return modifyValue + _triggerEffect.GetValue(modifyValue);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.CONSUME_MARK);
            buffer.Write(_markId);
            _triggerEffect.Serialize(buffer);
        }

        public ActionRecord ApplyEffect(EffectApplicationData data, float modification)
        {
            if (data.Target.HasStatus(_markId) == false)
            {
                return new DummyActionRecord();
            }

            ActionRecord result = _triggerEffect.ApplyEffect(data, modification);
            data.Target.RemoveStatus(_markId);
            return result;
        }
    }
}
