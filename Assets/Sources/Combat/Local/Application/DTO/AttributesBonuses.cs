using Combat.Common.ValueObjects;
using Combat.Local.Application.ValueObjects;

namespace Combat.Local.Application.DTO
{
    public ref struct AttributesBonuses
    {
        public AttributeModifcation Attack { get; set; }

        public AttributeModifcation Spellpower { get; set; }

        public AttributeModifcation Speed { get; set; }

        public float TimeScale { get; set; }

        public readonly AttributeModifcation this[UnitAttribute attribute] => attribute switch
        {
            UnitAttribute.Atk => Attack,
            UnitAttribute.Spellpower => Spellpower,
            UnitAttribute.Speed => Speed,
            _ => default,
        };

        public static AttributesBonuses operator +(AttributesBonuses value1, AttributesBonuses value2) => new()
        {
            Attack = value1.Attack + value2.Attack,
            Spellpower = value1.Spellpower + value2.Spellpower,
            Speed = value1.Speed + value2.Speed
        };
    }
}
