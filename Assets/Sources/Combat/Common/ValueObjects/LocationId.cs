using System;

namespace Lobby.Common.Primitives
{
    public readonly struct LocationId : IEquatable<LocationId>
    {
        public readonly int Value;

        public LocationId(int value)
        {
            Value = value;
        }

        public override bool Equals(object obj) => obj is LocationId id && Equals(id);
        public bool Equals(LocationId other) => Value == other.Value;
        public override int GetHashCode() => HashCode.Combine(Value);

        public static bool operator ==(LocationId left, LocationId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LocationId left, LocationId right)
        {
            return !(left == right);
        }
    }
}