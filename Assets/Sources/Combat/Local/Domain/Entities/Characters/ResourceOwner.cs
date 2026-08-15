using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities
{
    public readonly ref struct ResourceOwner
    {
        private readonly Span<ResourceValue> _values;

        public ResourceOwner(UnitId owner, Span<ResourceValue> values)
        {
            Id = owner;
            _values = values;
        }

        public UnitId Id { get; }

        public ResourceValue GetResource(ResourceId resource)
        {
            int index = FindResourceIndex(resource);

            if (index == -1)
            {
                return new(resource, default, default);
            }

            return _values[index];
        }

        public void SetResourceValue(ResourceId resource, float value)
        {
            int index = FindResourceIndex(resource);

            if (index == -1)
            {
                return;
            }

            ResourceValue currentValue = _values[index];
            float maxValue = currentValue.MaxValue;

            if (value > maxValue)
            {
                value = maxValue;
            }

            _values[index] = new(resource, value, maxValue);
            return;
        }

        public bool TrySpendResource(ResourceId resourceId, float value)
        {
            if (value < 0)
            {
                return false;
            }

            int index = FindResourceIndex(resourceId);

            if (index == -1)
            {
                return false;
            }

            ResourceValue resource = _values[index];

            if (resource.Value < value)
            {
                return false;
            }

            _values[index] = new(resourceId, resource.Value - value, resource.MaxValue);
            return true;
        }

        public void FillResource(ResourceId resourceId, float value)
        {
            if (value < 0)
            {
                return;
            }

            int index = FindResourceIndex(resourceId);

            if (index == -1)
            {
                return;
            }

            ResourceValue resource = _values[index];

            if (resource.Value + value > resource.MaxValue)
            {
                _values[index] = new(resourceId, resource.MaxValue, resource.MaxValue);
                return;
            }

            _values[index] = new(resourceId, resource.Value + value, resource.MaxValue);
        }

        public ReadOnlySpan<ResourceValue> GetAll() => _values;

        private int FindResourceIndex(ResourceId resourceId)
        {
            for (int i = 0; i < _values.Length; i++)
            {
                if (_values[i].ResourceId != resourceId)
                {
                    continue;
                }

                return i;
            }

            return -1;
        }
    }
}
