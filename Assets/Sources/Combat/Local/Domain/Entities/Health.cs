using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public struct Health
    {
        public Health(EntityId id, float defaultHealth)
        {
            Id = id;
            DefaultHealth = defaultHealth;
            MaxHealth = defaultHealth;
            CurrentHealth = 0;
        }

        public EntityId Id { get; }
        public float DefaultHealth { get; set; }
        public float CurrentHealth { get; set; }
        public float MaxHealth { get; set; }
    }
}
