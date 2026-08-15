namespace Combat.Common.Primitives
{
    public readonly struct UnitId
    {
        public readonly int Value;

        public UnitId(int id)
        {
            Value = id;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is UnitId enitityId && enitityId.Value == Value;

        public static bool operator ==(UnitId value1, UnitId value2) => value1.Equals(value2);

        public static bool operator !=(UnitId value1, UnitId value2) => !value1.Equals(value2);
    }
}
