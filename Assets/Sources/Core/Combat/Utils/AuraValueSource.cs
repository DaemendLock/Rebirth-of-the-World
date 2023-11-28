using Core.Combat.Auras;
using System.IO;
using Utils.DataStructure;
using Utils.Interfaces;
using Utils.Serializer;

namespace Core.Combat.Utils.ValueSources
{
    public interface AuraValueSource : SerializableInterface
    {
        float Evaluate(Status status);
    }

    public class Constant : AuraValueSource
    {
        private float _value;

        public Constant(float value)
        {
            _value = value;
        }

        public Constant(BinaryReader source)
        {
            _value = source.ReadSingle();
        }

        public float Evaluate(Status status)
        {
            return _value;
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) AuraEffectValue.CONSTANT);
            buffer.Write(_value);
        }
    }

    public class StatProvider : AuraValueSource
    {
        private float _statPercent;
        private UnitStat _stat;

        public StatProvider(UnitStat stat, float statPercent)
        {
            _statPercent = statPercent;
            _stat = stat;
        }

        public StatProvider(BinaryReader source)
        {
            _stat = (UnitStat) source.ReadInt32();
            _statPercent = source.ReadSingle();
        }

        public float Evaluate(Status status)
        {
            return _statPercent * status.GetEffectiveStat(_stat);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) AuraEffectValue.STAT_PROVIDER);
            buffer.Write((int) _stat);
            buffer.Write(_statPercent);
        }
    }

    public class ValueSum : AuraValueSource
    {
        private AuraValueSource _value1;
        private AuraValueSource _value2;

        public ValueSum(AuraValueSource value1, AuraValueSource value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public ValueSum(BinaryReader source)
        {
            _value1 = Serializer.Deserialize<AuraValueSource>(source);
            _value2 = Serializer.Deserialize<AuraValueSource>(source);
        }

        public float Evaluate(Status status)
        {
            return _value1.Evaluate(status) + _value2.Evaluate(status);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) AuraEffectValue.VALUE_SUM);
            _value1.Serialize(buffer);
            _value2.Serialize(buffer);
        }
    }

    public class ValueMultiplication : AuraValueSource
    {
        private AuraValueSource _value1;
        private AuraValueSource _value2;

        public ValueMultiplication(AuraValueSource value1, AuraValueSource value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public ValueMultiplication(BinaryReader source)
        {
            _value1 = Serializer.Deserialize<AuraValueSource>(source);
            _value2 = Serializer.Deserialize<AuraValueSource>(source);
        }

        public float Evaluate(Status status)
        {
            return _value1.Evaluate(status) * _value2.Evaluate(status);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) AuraEffectValue.VALUE_MULTIPLY);
            _value1.Serialize(buffer);
            _value2.Serialize(buffer);
        }
    }
}
