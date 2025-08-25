namespace Combat.Common.ValueObjects
{
    public readonly struct ModelName
    {
        public readonly string Value;

        public ModelName(string name)
        {
            Value = name;
        }

        public override string ToString() => Value;

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is ModelName enitityId && enitityId.Value == Value;

        public static bool operator ==(ModelName value1, ModelName value2) => value1.Equals(value2);

        public static bool operator !=(ModelName value1, ModelName value2) => !value1.Equals(value2);
    }
}
