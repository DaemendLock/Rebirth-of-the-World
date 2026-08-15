using System;

namespace Combat.Common.Primitives
{
    public readonly struct CharacterKey : IEquatable<CharacterKey>
    {
        public readonly string Value;

        public CharacterKey(string value)
        {
            Value = value;
        }

        public override bool Equals(object obj) => obj is CharacterKey key && Equals(key);

        public bool Equals(CharacterKey other) => Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(CharacterKey left, CharacterKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CharacterKey left, CharacterKey right)
        {
            return !(left == right);
        }
    }
}
