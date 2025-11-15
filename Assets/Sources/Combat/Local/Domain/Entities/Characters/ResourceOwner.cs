using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct ResourceOwner
    {
        private readonly Span<ResourceValue> _values;

        public ResourceOwner(EntityId owner, Span<ResourceValue> values)
        {
            Owner = owner;
            _values = values;
        }

        public EntityId Owner { get; }

        public ResourceValue GetResource(ResourceId resource)
        {
            Span<ResourceValue> values = _values;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].ResourceId != resource)
                {
                    continue;
                }

                return values[i];
            }

            return new(resource, default, default);
        }

        public void SetResourceValue(ResourceId resource, float value)
        {
            Span<ResourceValue> values = _values;

            for (int i = 0; i < values.Length; i++)
            {
                ResourceValue currentValue = values[i];

                if (currentValue.ResourceId != resource)
                {
                    continue;
                }

                float maxValue = currentValue.MaxValue;

                if (value > maxValue)
                {
                    value = maxValue;
                }

                values[i] = new(resource, value, maxValue);
                return;
            }
        }

        public ReadOnlySpan<ResourceValue> GetAll() => _values;
    }

    public struct Resource
    {
        public Resource(EntityId owner, ResourceId resourceId, float maxValue, float currentValue)
        {
            Id = owner;
            ResourceId = resourceId;
            MaxValue = maxValue;
            CurrentValue = currentValue;
        }

        public EntityId Id { get; }
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
