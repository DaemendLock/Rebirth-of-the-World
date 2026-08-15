using System;

namespace Combat.Common.ValueObjects
{
    public readonly struct ScaleEffectId : IEquatable<ScaleEffectId>
    {
        public readonly int Value;
        public readonly UnitId OwnerId;

        public ScaleEffectId(int value, UnitId ownerId)
        {
            Value = value;
            OwnerId = ownerId;
        }

        public override bool Equals(object obj) => obj is ScaleEffectId id && Equals(id);
        public bool Equals(ScaleEffectId other) => (Value == other.Value) && (OwnerId == other.OwnerId);
        public override int GetHashCode() => HashCode.Combine(Value, OwnerId);

        public static bool operator ==(ScaleEffectId left, ScaleEffectId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ScaleEffectId left, ScaleEffectId right)
        {
            return !(left == right);
        }
    }
}
