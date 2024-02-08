using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.DataStructure;

namespace Core.Combat.Abilities.SpellEffects
{
    public class AbsorbDamage : SpellEffect
    {
        private readonly SpellValueSource _value;
        private readonly SchoolType _schoolType;

        public AbsorbDamage(SpellValueSource value, SchoolType type)
        {
            _value = value;
            _schoolType = type;
        }

        public AbsorbDamage(ByteReader source)
        {
            _value = SpellSerializer.DeserializeSpellValue(source);
            _schoolType = (SchoolType) source.ReadUShort();
        }

        public float GetValue(float modifyValue) => _value.BaseValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.ABSORB_DAMAGE);
            _value.Serialize(buffer);
            buffer.Write((ushort) _schoolType);
        }

        public ActionRecord ApplyEffect(Unit caster, Unit target, float modification)
        {
            throw new System.NotImplementedException();
        }
    }
}
