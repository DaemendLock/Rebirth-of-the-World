namespace Combat.Common.ValueObjects
{
    public readonly struct EntityId
    {
        public readonly int Value;

        public EntityId(int id)
        {
            Value = id;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is EntityId enitityId && enitityId.Value == Value;

        public static bool operator ==(EntityId value1, EntityId value2) => value1.Equals(value2);

        public static bool operator !=(EntityId value1, EntityId value2) => !value1.Equals(value2);
    }
}
