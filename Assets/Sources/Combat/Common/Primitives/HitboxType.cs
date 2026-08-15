using System;

namespace Combat.Common.Primitives
{
    public readonly struct HitboxType
    {
        public HitboxType(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override bool Equals(object obj) => obj is HitboxType type && Value == type.Value;

        public override int GetHashCode() => HashCode.Combine(Value);

        public static bool operator ==(HitboxType hitboxType, object obj) => hitboxType.Equals(obj);

        public static bool operator !=(HitboxType hitboxType, object obj) => !hitboxType.Equals(obj);

        public override string ToString() => Value;
    }
}
