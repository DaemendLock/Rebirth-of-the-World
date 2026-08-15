using System;

namespace Combat.Common.Primitives
{
    public readonly struct HitHandlerId : IEquatable<HitHandlerId>
    {
        public readonly int Value;

        public HitHandlerId(int id)
        {
            Value = id;
        }

        public override bool Equals(object obj) => obj is HitHandlerId id && Equals(id);
        public bool Equals(HitHandlerId other) => Value == other.Value;
        public override int GetHashCode() => Value;

        public static bool operator ==(HitHandlerId left, HitHandlerId right) => left.Equals(right);

        public static bool operator !=(HitHandlerId left, HitHandlerId right) => !(left == right);
    }
}
