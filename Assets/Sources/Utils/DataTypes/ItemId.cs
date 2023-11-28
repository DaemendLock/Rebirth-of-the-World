namespace Utils.DataTypes
{
    public readonly struct ItemId
    {
        private readonly int _id;

        public ItemId(int id) { _id = id; }

        public static bool operator ==(ItemId obj1, ItemId obj2)
        {
            return obj1._id == obj2._id;
        }

        public static bool operator !=(ItemId obj1, ItemId obj2)
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
            return obj is ItemId spellId && spellId._id == _id;
        }

        public static explicit operator ItemId(int id)
        {
            return new ItemId(id);
        }

        public static implicit operator int(ItemId spellId)
        {
            return spellId._id;
        }
    }
}
