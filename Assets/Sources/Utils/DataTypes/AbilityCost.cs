using System;

namespace Utils.DataTypes
{
    [Serializable]
    public readonly struct AbilityCost
    {
        public readonly static AbilityCost None = new AbilityCost(0f, 0f);

        public readonly float Left;
        public readonly float Right;

        public AbilityCost(float left, float right)
        {
            Left = left;
            Right = right;
        }

        public static AbilityCost operator +(AbilityCost value1, AbilityCost value2) => new AbilityCost(value1.Left + value2.Left, value1.Right + value2.Right);
    }
}
