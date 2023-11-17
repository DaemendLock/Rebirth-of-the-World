using Core.Combat.Utils;
using System.IO;
using Utils.Serializer;

namespace Core.Combat.Abilities.SpellEffects
{
    public class AbsorbDamage : SpellEffect
    {
        private readonly ValueSource _value;
        private readonly SchoolType _schoolType;

        public AbsorbDamage(ValueSource value, SchoolType type)
        {
            _value = value;
            _schoolType = type;
        }

        public AbsorbDamage(BinaryReader source)
        {
            _value = SpellSerializer.DeserializeSpellValue(source);
            _schoolType = (SchoolType) source.ReadUInt16();
        }

        public float GetValue(float modifyValue) => _value.BaseValue;

        public void ApplyEffect(CastEventData data, float modifyValue)
        {
            data.Caster.AbsorbDamage(data, _value.GetValue(data, modifyValue), SchoolType.ANY);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.ABSORB_DAMAGE);
            _value.Serialize(buffer);
            buffer.Write((ushort)_schoolType);
        }
    }
}
