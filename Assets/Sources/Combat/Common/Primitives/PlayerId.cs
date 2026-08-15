using System;

namespace Combat.Common.Primitives
{
    public readonly struct PlayerId : IEquatable<PlayerId>
    {
        public readonly Guid Value;

        public PlayerId(Guid id)
        {
            Value = id;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is PlayerId enitityId && enitityId.Value == Value;

        public bool Equals(PlayerId other) => Value == other.Value;

        public static bool operator ==(PlayerId value1, PlayerId value2) => value1.Equals(value2);

        public static bool operator !=(PlayerId value1, PlayerId value2) => !value1.Equals(value2);
    }
}
