namespace Combat.Common.ValueObjects
{
    public readonly struct DamageInstanceId
    {
        public readonly int Value;

        public DamageInstanceId(int id)
        {
            Value = id;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is DamageInstanceId enitityId && enitityId.Value == Value;

        public static bool operator ==(DamageInstanceId value1, DamageInstanceId value2) => value1.Equals(value2);

        public static bool operator !=(DamageInstanceId value1, DamageInstanceId value2) => !value1.Equals(value2);
    }
}
