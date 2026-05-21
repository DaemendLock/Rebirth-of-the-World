using System;

namespace Lobby.Common.Primitives
{
    public readonly struct EncounterId : IEquatable<EncounterId>
    {
        public readonly Guid Value;

        public EncounterId(Guid value)
        {
            Value = value;
        }
        public override bool Equals(object obj) => obj is EncounterId id && Equals(id);
        public bool Equals(EncounterId other) => Value.Equals(other.Value);
        public override int GetHashCode() => HashCode.Combine(Value);

        public static bool operator ==(EncounterId left, EncounterId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EncounterId left, EncounterId right)
        {
            return !(left == right);
        }
    }
}