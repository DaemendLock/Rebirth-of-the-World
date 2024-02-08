using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Abilities.SpellEffects
{
    public class ApplyAura : SpellEffect
    {
        private readonly SpellId _effectId;
        private readonly int _value;

        public ApplyAura(SpellId effectId, int value = 0)
        {
            _effectId = effectId;
            _value = value;
        }

        public ApplyAura(ByteReader source)
        {
            _effectId = (SpellId) source.ReadInt();
            _value = source.ReadInt();
        }

        public float GetValue(float modifyValue)
        {
            return modifyValue + _value;
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.APPLY_AURA);
            buffer.Write(_effectId);
            buffer.Write(_value);
        }

        public ActionRecord ApplyEffect(Unit caster, Unit target, float modification)
        {
            throw new System.NotImplementedException();
        }
    }
}