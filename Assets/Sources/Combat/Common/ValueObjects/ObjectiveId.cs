using System;

namespace Combat.Common.ValueObjects
{
    public readonly struct ObjectiveId : IEquatable<ObjectiveId>
    {
        public readonly int Value;

        public ObjectiveId(int value)
        {
            Value = value;
        }

        public override bool Equals(object obj) => obj is ObjectiveId id && Equals(id);
        public bool Equals(ObjectiveId other) => Value == other.Value;
        public override int GetHashCode() => Value;
        public override string ToString() => Value.ToString();

        public static bool operator ==(ObjectiveId left, ObjectiveId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ObjectiveId left, ObjectiveId right)
        {
            return !(left == right);
        }
    }
}
