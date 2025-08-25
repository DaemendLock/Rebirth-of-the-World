using System;

namespace Combat.Common.ValueObjects
{
    public readonly struct HurtboxType
    {
        public HurtboxType(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override bool Equals(object obj) => obj is HurtboxType type && Value == type.Value;
        public override int GetHashCode() => HashCode.Combine(Value);

        public override string ToString() => Value;
    }
}
