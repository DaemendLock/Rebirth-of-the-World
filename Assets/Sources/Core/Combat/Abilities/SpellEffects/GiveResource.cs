using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.DataStructure;
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

        public ActionRecord ApplyEffect(Unit caster, Unit target, float value) => new ModifyResourceRecord(target.Id, _type, target.GiveResource(_type, value));

    }
}