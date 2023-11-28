using Core.Combat.Utils;
using System.IO;
using Utils.Serializer;

namespace Core.Combat.Abilities.SpellEffects
{
    public class Dummy : SpellEffect
    {
        private readonly float _value;

        public Dummy(float value)
        {
            _value = value;
        }

        public Dummy(BinaryReader source)
        {
            _value = source.ReadSingle();
        }

        public float GetValue(float modifyValue) => _value + modifyValue;

        public void ApplyEffect(CastEventData data, float modifyValue)
        {

        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.DUMMY);
            buffer.Write(_value);
        }
    }
}