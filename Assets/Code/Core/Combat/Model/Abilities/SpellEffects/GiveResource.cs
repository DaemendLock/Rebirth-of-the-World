using Core.Combat.Units;
using Core.Combat.Utils;
using System.IO;

namespace Core.Combat.Abilities.SpellEffects
{
    public class GiveResource : SpellEffect
    {
        private float _value;
        private ResourceType _type;

        public GiveResource(float value, ResourceType type)
        {
            _value = value;
            _type = type;
        }

        public GiveResource(BinaryReader source)
        {
            _value = source.ReadSingle();
            _type = (ResourceType) source.ReadUInt16();
        }

        public void ApplyEffect(EventData data, float modifyValue)
        {
            data.Target.GiveResource(_type, GetValue(modifyValue));
        }

        public float GetValue(float modifyValue) => _value + modifyValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            buffer.Write(_value);
            buffer.Write((ushort) _type);
        }
    }
}