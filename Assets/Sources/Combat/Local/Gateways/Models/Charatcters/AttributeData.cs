using Combat.Common.ValueObjects;

namespace Combat.Local.Gateways.Models
{
    public readonly struct AttributeData
    {
        public readonly AttributeValue[] BaseValues;
        public readonly float[] Values;

        public AttributeData(AttributeValue[] baseValues, float[] values)
        {
            BaseValues = baseValues;
            Values = values;
        }
    }
}
