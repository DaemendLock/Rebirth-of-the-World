using System;

namespace Combat.Common.Primitives
{
    public readonly struct EventHandlerId : IEquatable<EventHandlerId>
    {
        public readonly int Value;

        public EventHandlerId(int id)
        {
            Value = id;
        }

        public override bool Equals(object obj) => obj is EventHandlerId id && Equals(id);

        public bool Equals(EventHandlerId other) => Value == other.Value;

        public override int GetHashCode() => Value;

        public override string ToString() => Value.ToString();

        public static bool operator ==(EventHandlerId left, EventHandlerId right) => left.Equals(right);

        public static bool operator !=(EventHandlerId left, EventHandlerId right) => !(left == right);
    }
}
