using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.OutputPorts
{
    public readonly ref struct GiveResourceResult
    {
        public GiveResourceResult(UnitId target, ResourceId resource, float value, float currentValue, float maxValue, AbilityKey? skill)
        {
            Target = target;
            Resource = resource;
            Value = value;
            AbilityId = skill;
            CurrentValue = currentValue;
            MaxValue = maxValue;
        }

        public UnitId Target { get; }
        public ResourceId Resource { get; }
        public float Value { get; }
        public float CurrentValue { get; }
        public float MaxValue { get; }
        public AbilityKey? AbilityId { get; }
    }

    public interface IGiveResourceOutput
    {
        void Present(GiveResourceResult resource);
    }
}
