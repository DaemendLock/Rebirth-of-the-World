using System;

namespace Combat.Common.ValueObjects
{
    public readonly struct EventHandlerId : IEquatable<EventHandlerId>
    {
        public readonly int Id;

        public EventHandlerId(int id)
        {
            Id = id;
        }

        public override bool Equals(object obj) => obj is EventHandlerId id && Equals(id);

        public bool Equals(EventHandlerId other) => Id == other.Id;

        public override int GetHashCode() => Id;

        public override string ToString() => Id.ToString();

        public static bool operator ==(EventHandlerId left, EventHandlerId right) => left.Equals(right);

        public static bool operator !=(EventHandlerId left, EventHandlerId right) => !(left == right);
    }
}
