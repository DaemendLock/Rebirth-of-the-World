using Core.Combat.Auras;
using Core.Stats;
using System.IO;
using Utils.Interfaces;
using Utils.Serializer;

namespace Core.Combat.Utils.ValueSources
{
    public interface ValueSource : SerializableInterface
    {
        float Evaluate(Status status);
    }

    public class Constant : ValueSource
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
            buffer.Write(GetType().FullName);
            buffer.Write(_value);
        }
    }

    public class StatProvider : ValueSource
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
            buffer.Write(GetType().FullName);
            buffer.Write((int)_stat);
            buffer.Write(_statPercent);
        }
    }

    public class ValueSum : ValueSource
    {
        private ValueSource _value1;
        private ValueSource _value2;

        public ValueSum(ValueSource value1, ValueSource value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public ValueSum(BinaryReader source)
        {
            _value1 = Serializer.Deserialize<ValueSource>(source);
            _value2 = Serializer.Deserialize<ValueSource>(source);
        }

        public float Evaluate(Status status)
        {
            return _value1.Evaluate(status) + _value2.Evaluate(status);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().FullName);
            _value1.Serialize(buffer);
            _value2.Serialize(buffer);
        }
    }

    public class ValueMultiplication : ValueSource
    {
        private ValueSource _value1;
        private ValueSource _value2;

        public ValueMultiplication(ValueSource value1, ValueSource value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public ValueMultiplication(BinaryReader source)
        {
            _value1 = Serializer.Deserialize<ValueSource>(source);
            _value2 = Serializer.Deserialize<ValueSource>(source);
        }

        public float Evaluate(Status status)
        {
            return _value1.Evaluate(status) * _value2.Evaluate(status);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().FullName);
            _value1.Serialize(buffer);
            _value2.Serialize(buffer);
        }
    }
}
