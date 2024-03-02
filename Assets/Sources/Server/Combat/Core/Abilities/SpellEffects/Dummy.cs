using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.ByteHelper;

namespace Core.Combat.Abilities.SpellEffects
{
    public class Dummy : SpellEffect
    {
        private readonly float _value;

        public Dummy(float value)
        {
            _value = value;
        }

        public Dummy(ByteReader source)
        {
            _value = source.ReadFloat();
        }

        public ActionRecord ApplyEffect(EffectApplicationData data, float modification) => new DummyActionRecord();

        public float GetValue(float modifyValue) => _value + modifyValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.DUMMY);
            buffer.Write(_value);
        }
    }
}