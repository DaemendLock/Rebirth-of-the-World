using System;

namespace Combat.Common.Primitives
{

    public readonly struct ActionId : IEquatable<ActionId>
    {
        public readonly int Value;

        public ActionId(int id)
        {
            Value = id;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is ActionId enitityId && enitityId.Value == Value;

        public bool Equals(ActionId other) => Value == other.Value;

        public static bool operator ==(ActionId value1, ActionId value2) => value1.Equals(value2);

        public static bool operator !=(ActionId value1, ActionId value2) => !value1.Equals(value2);
    }
}
