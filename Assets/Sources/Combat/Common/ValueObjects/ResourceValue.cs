using Combat.Common.Primitives;

namespace Combat.Common.ValueObjects
{
    public readonly struct ResourceValue
    {
        public readonly ResourceId ResourceId;
        public readonly float Value;
        public readonly float MaxValue;

        public ResourceValue(ResourceId resourceId, float value, float maxValue)
        {
            ResourceId = resourceId;
            Value = value;
            MaxValue = maxValue;
        }
    }
}
