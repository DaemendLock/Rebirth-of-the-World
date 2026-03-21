namespace Combat.Common.ValueObjects
{
    public readonly struct TransformEffectId
    {
        public readonly int Value;

        public TransformEffectId(int id)
        {
            Value = id;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is TransformEffectId enitityId && enitityId.Value == Value;

        public static bool operator ==(TransformEffectId value1, TransformEffectId value2) => value1.Equals(value2);

        public static bool operator !=(TransformEffectId value1, TransformEffectId value2) => !value1.Equals(value2);
    }
}
