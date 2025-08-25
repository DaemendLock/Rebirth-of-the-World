namespace Combat.Common.ValueObjects
{
    public readonly struct StatusName
    {
        public readonly string Value;

        public StatusName(string value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is StatusName enitityId && enitityId.Value == Value;

        public static bool operator ==(StatusName value1, StatusName value2) => value1.Equals(value2);

        public static bool operator !=(StatusName value1, StatusName value2) => !value1.Equals(value2);
    }
}
