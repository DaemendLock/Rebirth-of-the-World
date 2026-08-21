using System;

namespace Combat.Common.Primitives
{
    public readonly struct AuraId : IEquatable<AuraId>
    {
        public readonly int Value;

        public AuraId(int value)
        {
            Value = value;
        }

        public override bool Equals(object obj) => obj is AuraId id && Equals(id);

        public bool Equals(AuraId other) => Value == other.Value;

        public override int GetHashCode() => Value;

        public static bool operator ==(AuraId left, AuraId right) => left.Equals(right);

        public static bool operator !=(AuraId left, AuraId right) => !(left == right);
    }
}
