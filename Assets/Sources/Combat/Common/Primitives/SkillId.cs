namespace Combat.Common.Primitives
{
    public readonly struct SkillId
    {
        public readonly int Value;

        public SkillId(int id)
        {
            Value = id;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is SkillId enitityId && enitityId.Value == Value;

        public static bool operator ==(SkillId value1, SkillId value2) => value1.Equals(value2);

        public static bool operator !=(SkillId value1, SkillId value2) => !value1.Equals(value2);
    }
}
