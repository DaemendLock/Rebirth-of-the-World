using Combat.Common.Primitives;

namespace Combat.API.DTO
{
    public readonly ref struct GiveResourceInfo
    {
        public GiveResourceInfo(ResourceId resource, float value, AbilityApi source)
        {
            Resource = resource;
            Value = value;
            Source = source;
        }

        public ResourceId Resource { get; }
        public float Value { get; }
        public AbilityApi Source { get; }
    }
}
