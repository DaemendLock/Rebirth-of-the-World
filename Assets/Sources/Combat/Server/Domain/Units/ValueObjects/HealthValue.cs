namespace Server.Combat.Domain.Units.ValueObjects
{
    public ref struct HealthValue
    {
        public HealthValue(float currentHealth, bool alive, float maxHealth)
        {
            CurrentHealth = currentHealth;
            Alive = alive;
            DefaultHealth = maxHealth;
        }

        public float CurrentHealth { get; set; }
        public float DefaultHealth { get; set; }
        public bool Alive { get; set; }
    }
}
