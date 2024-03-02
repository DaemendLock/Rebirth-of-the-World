using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.ByteHelper;
using Utils.DataTypes;

namespace Core.Combat.Abilities.SpellEffects
{
    public class GiveResource : SpellEffect
    {
        private readonly float _value;
        private readonly ResourceType _type;

        public GiveResource(float value, ResourceType type)
        {
            _value = value;
            _type = type;
        }

        public GiveResource(ByteReader source)
        {
            _value = source.ReadFloat();
            _type = (ResourceType) source.ReadUShort();
        }

        public float GetValue(float modifyValue) => _value + modifyValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.GIVE_RESOURCE);
            buffer.Write(_value);
            buffer.Write((ushort) _type);
        }

        public ActionRecord ApplyEffect(EffectApplicationData data, float modification) => new ModifyResourceRecord(data, _type, data.Target.GiveResource(_type, _value + modification));

    }
}