namespace Utils.DataTypes
{
    public readonly struct SpellId
    {
        private readonly int _id;

        public SpellId(int id) { _id = id; }

        public static bool operator ==(SpellId obj1, SpellId obj2)
        {
            return obj1._id == obj2._id;
        }

        public static bool operator !=(SpellId obj1, SpellId obj2)
        {
            return obj1._id != obj2._id;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public override string ToString()
        {
            return _id.ToString();
        }
        public override bool Equals(object obj)
        {
            return obj is SpellId spellId && spellId._id == _id;
        }

        public static explicit operator SpellId(int id)
        {
            return new SpellId(id);
        }

        public static implicit operator int(SpellId spellId)
        {
            return spellId._id;
        }
    }
}
