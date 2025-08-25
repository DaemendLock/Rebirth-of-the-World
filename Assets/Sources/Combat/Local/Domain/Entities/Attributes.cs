using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public readonly struct Attributes
    {
        public const int AttributeCount = (int) (Attribute.PARRY + 1);

        private readonly AttributeValue[] _baseValues;
        private readonly AttributeValue[] _bonuses;

        public Attributes(EntityId id, AttributeValue[] baseValues)
        {
            Id = id;

            _baseValues = new AttributeValue[AttributeCount];
            _bonuses = new AttributeValue[AttributeCount];
            baseValues.CopyTo(_baseValues, 0);
        }

        public EntityId Id { get; }

        public AttributeValue this[Attribute attribute] => _baseValues[(int) attribute] + _bonuses[(int) attribute];

        public AttributeValue GetBaseValue(Attribute attribute) => _baseValues[(int) attribute];

        public void WriteBonues(AttributeValue[] bonuses) => bonuses.CopyTo(_bonuses, 0);
    }
}
