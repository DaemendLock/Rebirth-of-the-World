using Combat.Common.ValueObjects;
using Combat.Local.Application.ValueObjects;

namespace Combat.Local.Application.DTO
{
    public readonly ref struct StatusBonuses
    {
        public readonly AttributesBonuses Attributes;
        public readonly float TimeScale;
        public readonly AttributeModifcation Movespeed;

        public StatusBonuses(AttributesBonuses attributes, float timeScale, AttributeModifcation movespeed)
        {
            Attributes = attributes;
            TimeScale = timeScale;
            Movespeed = movespeed;
        }
    }

    public ref struct AttributesBonuses
    {
        public AttributeModifcation Attack { get; set; }

        public AttributeModifcation Spellpower { get; set; }

        public AttributeModifcation Haste { get; set; }

        public AttributeModifcation Lethality { get; set; }

        public AttributeModifcation Versatility { get; set; }

        public AttributeModifcation Impact { get; set; }

        public AttributeModifcation Endurance { get; set; }

        public AttributeModifcation Swiftness { get; set; }

        public readonly AttributeModifcation this[UnitAttribute attribute] => attribute switch
        {
            UnitAttribute.Atk => Attack,
            UnitAttribute.Spellpower => Spellpower,
            UnitAttribute.Haste => Haste,
            UnitAttribute.Lethality => Lethality,
            UnitAttribute.Versality => Versatility,
            UnitAttribute.Impact => Impact,
            UnitAttribute.Endurance => Endurance,
            UnitAttribute.Swiftness => Swiftness,
            _ => default,
        };

        public static AttributesBonuses operator +(AttributesBonuses value1, AttributesBonuses value2) => new()
        {
            Attack = value1.Attack + value2.Attack,
            Spellpower = value1.Spellpower + value2.Spellpower,
            Haste = value1.Haste + value2.Haste,
            Lethality = value1.Lethality + value2.Lethality,
            Versatility = value1.Versatility + value2.Versatility,
            Impact = value1.Impact + value2.Impact,
            Endurance = value1.Endurance + value2.Endurance,
            Swiftness = value1.Swiftness + value2.Swiftness
        };
    }
}
