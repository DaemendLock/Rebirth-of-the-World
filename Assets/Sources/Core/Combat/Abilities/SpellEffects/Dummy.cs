using Core.Combat.Utils;
using System.IO;

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

        public void ApplyEffect(EventData data, float modifyValue)
        {

        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            buffer.Write(_value);
        }
    }
}