using System;

namespace Lobby.Common.Primitives
{
    public readonly struct CharacterId : IEquatable<CharacterId>
    {
        public readonly int Value;

        public CharacterId(int value)
        {
            Value = value;
        }

        public override bool Equals(object obj) => obj is CharacterId id && Equals(id);

        public bool Equals(CharacterId other) => Value == other.Value;

        public override int GetHashCode() => HashCode.Combine(Value);

        public static bool operator ==(CharacterId left, CharacterId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CharacterId left, CharacterId right)
        {
            return !(left == right);
        }

        public override string ToString() => Value.ToString();
    }
}