namespace Server.Combat.Domain.Units.ValueObjects
{
    public readonly struct UnitId
    {
        public readonly int Id;

        public UnitId(int id)
        {
            Id = id;
        }

        public override string ToString() => Id.ToString();

        public override int GetHashCode() => Id.GetHashCode();

        public override bool Equals(object obj) => obj is UnitId enitityId && enitityId.Id == Id;

        public static bool operator ==(UnitId value1, UnitId value2) => value1.Equals(value2);

        public static bool operator !=(UnitId value1, UnitId value2) => !value1.Equals(value2);
    }
}
