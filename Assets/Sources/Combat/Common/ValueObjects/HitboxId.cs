using System;

namespace Combat.Common.ValueObjects
{
    public readonly struct HitboxId
    {
        public HitboxId(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public override bool Equals(object obj) => obj is HitboxId id && Value == id.Value;
        public override int GetHashCode() => HashCode.Combine(Value);
        public override string ToString() => Value.ToString();
    }
}
