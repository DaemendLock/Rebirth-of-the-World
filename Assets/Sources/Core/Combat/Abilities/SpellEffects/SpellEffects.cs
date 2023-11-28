using Core.Combat.Units;
using Core.Combat.Utils;
using System.IO;
using Utils.DataStructure;
using Utils.Interfaces;
using Utils.Serializer;

namespace Core.Combat.Abilities.SpellEffects
{
    public interface SpellEffect : SerializableInterface
    {
        public float GetValue(float modifyValue);

        public void ApplyEffect(CastEventData data, float modifyValue);
    }

    public interface SpellValueSource : SerializableInterface
    {
        float BaseValue { get; }

        float GetValue(CastEventData data, float modifyValue);
    }

    public class FixedValue : SpellValueSource
    {
        public FixedValue(float value)
        {
            BaseValue = value;
        }

        public FixedValue(BinaryReader source)
        {
            BaseValue = source.ReadSingle();
        }

        public float BaseValue { get; }

        public float GetValue(CastEventData data, float modifyValue) => BaseValue + modifyValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectValue.FIXED_VALUE);
            buffer.Write(BaseValue);
        }
    }

    public readonly struct StatValue : SpellValueSource
    {
        private readonly UnitStat _stat;

        public StatValue(float spellpowerMultiplier, UnitStat stat)
        {
            BaseValue = spellpowerMultiplier;
            _stat = stat;
        }

        public StatValue(BinaryReader source)
        {
            BaseValue = source.ReadSingle();
            _stat = (UnitStat) source.ReadInt32();
        }

        public float BaseValue { get; }

        public float GetValue(CastEventData data, float modifyValue) => data.Caster.EvaluateStat(_stat).CalculatedValue * (BaseValue + modifyValue);

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

        public CasterResourceValue(BinaryReader source)
        {
            BaseValue = source.ReadSingle();
        }

        public float BaseValue { get; }

        public float GetValue(CastEventData data, float modifyValue) => data.Caster.GetResourceValue((ResourceType) (BaseValue + modifyValue));

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

        public MultiplyValue(BinaryReader source)
        {
            _value1 = SpellSerializer.DeserializeSpellValue(source);
            _value2 = SpellSerializer.DeserializeSpellValue(source);
        }

        public float BaseValue => _value1.BaseValue * _value2.BaseValue;

        public float GetValue(CastEventData data, float modifyValue)
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