namespace Combat.Common.ValueObjects
{
    public readonly struct StatusId
    {
        public readonly int Value;

        public StatusId(int id)
        {
            Value = id;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is StatusId enitityId && enitityId.Value == Value;

        public static bool operator ==(StatusId value1, StatusId value2) => value1.Equals(value2);

        public static bool operator !=(StatusId value1, StatusId value2) => !value1.Equals(value2);
    }
}
