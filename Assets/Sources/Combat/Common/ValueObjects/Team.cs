namespace Combat.Common.ValueObjects
{
    public readonly struct Team
    {
        public readonly byte Value;

        public Team(byte id)
        {
            Value = id;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is Team enitityId && enitityId.Value == Value;

        public static bool operator ==(Team value1, Team value2) => value1.Value == value2.Value;

        public static bool operator !=(Team value1, Team value2) => !value1.Equals(value2);
    }
}
