using System;

namespace Combat.Common.ValueObjects
{
    public readonly struct ProjectileId : IEquatable<ProjectileId>
    {
        public readonly int Value;

        public ProjectileId(int value)
        {
            Value = value;
        }

        public override bool Equals(object obj) => obj is ProjectileId id && Equals(id);

        public bool Equals(ProjectileId other) => Value == other.Value;

        public override int GetHashCode() => HashCode.Combine(Value);

        public static bool operator ==(ProjectileId left, ProjectileId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ProjectileId left, ProjectileId right)
        {
            return !(left == right);
        }

        public override string ToString() => Value.ToString();
    }
}
