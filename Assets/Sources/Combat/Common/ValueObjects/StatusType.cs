namespace Combat.Common.ValueObjects
{
    public readonly struct StatusType
    {
        public readonly string Value;

        public StatusType(string value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is StatusType enitityId && enitityId.Value == Value;

        public static bool operator ==(StatusType value1, StatusType value2) => value1.Equals(value2);

        public static bool operator !=(StatusType value1, StatusType value2) => !value1.Equals(value2);
    }
}
