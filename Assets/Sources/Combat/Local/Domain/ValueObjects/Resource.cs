using Combat.Common.Primitives;

namespace Combat.Local.Domain.ValueObjects
{
    public struct Resource
    {
        public Resource(UnitId owner, ResourceId resourceId, float maxValue, float currentValue)
        {
            Id = owner;
            ResourceId = resourceId;
            MaxValue = maxValue;
            CurrentValue = currentValue;
        }

        public UnitId Id { get; }
        public ResourceId ResourceId { get; }
        public float MaxValue { get; set; }
        public float CurrentValue { get; set; }

        public void Spend(float value)
        {
            if (value < 0)
            {
                return;
            }

            if (CurrentValue < value)
            {
                CurrentValue = 0;
                return;
            }

            CurrentValue -= value;
        }

        public void Fill(float value)
        {
            if (value < 0)
            {
                return;
            }

            if (CurrentValue + value > MaxValue)
            {
                CurrentValue = MaxValue;
                return;
            }

            CurrentValue += value;
        }
    }
}
