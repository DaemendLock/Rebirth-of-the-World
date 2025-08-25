using System;

namespace Combat.Common.ValueObjects
{
    public readonly struct HurtboxId
    {
        public HurtboxId(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public override bool Equals(object obj) => obj is HurtboxId id && Value == id.Value;
        public override int GetHashCode() => HashCode.Combine(Value);

        public override string ToString() => Value.ToString();
    }
}
