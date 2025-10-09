namespace Combat.Local.Data.Models
{
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
