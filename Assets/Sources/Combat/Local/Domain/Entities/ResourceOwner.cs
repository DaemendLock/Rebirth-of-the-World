using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public struct Resource
    {
        public Resource(EntityId owner, ResourceId resourceId, float maxValue, float currentValue)
        {
            Owner = owner;
            ResourceId = resourceId;
            MaxValue = maxValue;
            CurrentValue = currentValue;
        }

        public EntityId Owner { get; }
        public ResourceId ResourceId { get; }
        public float MaxValue { get; set; }
        public float CurrentValue { get; set; }
    }
}
