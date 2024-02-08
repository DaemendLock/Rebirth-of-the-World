using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.DataStructure;
using Utils.DataTypes;
using Utils.Interfaces;

namespace Core.Combat.Abilities.SpellEffects
{
    public class SpellEffectValueProvider
    {
        private float _effectModification;

        public SpellEffectValueProvider(Unit caster, SpellId source, float effectModification)
        {
            _effectModification = effectModification;
            Caster = caster;
            Source = source;
        }

        public Unit Caster { get; }

        public SpellId Source { get; }

        public float GetValue(SpellEffect effect) => effect.GetValue(_effectModification);
    }

    public interface SpellEffect : SerializableInterface
    {
        float GetValue(float modifyValue);

        ActionRecord ApplyEffect(Unit caster, Unit target, float modification);
    }

    public interface SpellValueSource : SerializableInterface
    {
        float BaseValue { get; }

        float GetValue(CastInputData data, float modifyValue);
    }

    public class FixedValue : SpellValueSource
    {
        public FixedValue(float value)
        {
            BaseValue = value;
        }

        public FixedValue(ByteReader source)
        {
            BaseValue = source.ReadFloat();
        }

        public float BaseValue { get; }

        public float GetValue(CastInputData data, float modifyValue) => BaseValue + modifyValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectValue.FIXED_VALUE);
            buffer.Write(BaseValue);
        }
    }

    public readonly struct StatValue : SpellValueSource
    {
        private readonly UnitStat _stat;

        public StatValue(float statMultiplier, UnitStat stat)
        {
            BaseValue = statMultiplier;
            _stat = stat;
        }

        public StatValue(ByteReader source)
        {
            BaseValue = source.ReadFloat();
            _stat = (UnitStat) source.ReadInt();
        }

        public float BaseValue { get; }

        public float GetValue(CastInputData data, float modifyValue) => data.Caster.EvaluateStat(_stat).CalculatedValue * (BaseValue + modifyValue);

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectValue.CASTER_STAT);
            buffer.Write(BaseValue);
            buffer.Write((int) _stat);
        }
    }

    public readonly struct CasterResourceValue : SpellValueSource
    {
        public CasterResourceValue(ResourceType resource)
        {
            BaseValue = (ushort) resource;
        }

        public CasterResourceValue(ByteReader source)
        {
            BaseValue = source.ReadFloat();
        }

        public float BaseValue { get; }

        public float GetValue(CastInputData data, float modifyValue) => data.Caster.GetResourceValue((ResourceType) (BaseValue + modifyValue));

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectValue.CASTER_RESOURCE);
            buffer.Write(BaseValue);
        }
    }

    public readonly struct MultiplyValue : SpellValueSource
    {
        private readonly SpellValueSource _value1;
        private readonly SpellValueSource _value2;

        public MultiplyValue(SpellValueSource value1, SpellValueSource value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public MultiplyValue(ByteReader source)
        {
            _value1 = SpellSerializer.DeserializeSpellValue(source);
            _value2 = SpellSerializer.DeserializeSpellValue(source);
        }

        public float BaseValue => _value1.BaseValue * _value2.BaseValue;

        public float GetValue(CastInputData data, float modifyValue)
        {
            return _value1.GetValue(data, modifyValue) * _value2.GetValue(data, 0);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectValue.MULTIPLY);
            _value1.Serialize(buffer);
            _value2.Serialize(buffer);
        }
    }
}