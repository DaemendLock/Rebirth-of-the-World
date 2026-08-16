using System;

namespace Lobby.Common.Primitives
{
    public readonly struct CharacterKey : IEquatable<CharacterKey>
    {
        public readonly string Value;

        public CharacterKey(string value)
        {
            Value = value;
        }

        public override bool Equals(object obj) => obj is CharacterKey id && Equals(id);

        public bool Equals(CharacterKey other) => Value == other.Value;

        public override int GetHashCode() => HashCode.Combine(Value);

        public static bool operator ==(CharacterKey left, CharacterKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CharacterKey left, CharacterKey right)
        {
            return !(left == right);
        }

        public override string ToString() => Value.ToString();
    }
}