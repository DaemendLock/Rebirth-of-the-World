namespace Combat.Common.ValueObjects
{
    public readonly struct ResourceId
    {
        public readonly static ResourceId Energy = new(1);
        public readonly static ResourceId Custom = new(2);

        public readonly int Value;

        public ResourceId(int id)
        {
            Value = id;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is ResourceId enitityId && enitityId.Value == Value;

        public static bool operator ==(ResourceId value1, ResourceId value2) => value1.Equals(value2);

        public static bool operator !=(ResourceId value1, ResourceId value2) => !value1.Equals(value2);
    }
}