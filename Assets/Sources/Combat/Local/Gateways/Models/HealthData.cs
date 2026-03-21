using Combat.Common.ValueObjects;

namespace Combat.Local.Data.Models
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

    public readonly struct HealthData
    {
        public HealthData(float currentHealth, float defaultHealth)
        {
            DefaultHealth = defaultHealth;
            CurrentHealth = currentHealth;
        }

        public float CurrentHealth { get; }
        public float DefaultHealth { get; }
    }
}
