using System;

namespace Combat.Common.Primitives
{
    public readonly struct ActionContextId : IEquatable<ActionContextId>
    {
        public readonly int Value;

        public ActionContextId(int id)
        {
            Value = id;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is ActionContextId enitityId && enitityId.Value == Value;

        public bool Equals(ActionContextId other) => Value == other.Value;

        public static bool operator ==(ActionContextId value1, ActionContextId value2) => value1.Equals(value2);

        public static bool operator !=(ActionContextId value1, ActionContextId value2) => !value1.Equals(value2);
    }
}
