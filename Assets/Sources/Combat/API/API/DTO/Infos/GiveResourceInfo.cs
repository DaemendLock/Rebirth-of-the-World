using Combat.Common.ValueObjects;

namespace Combat.API.DTO
{
    public readonly ref struct GiveResourceInfo
    {
        public GiveResourceInfo(ResourceId resource, float value, SkillApi source)
        {
            Resource = resource;
            Value = value;
            Source = source;
        }

        public ResourceId Resource { get; }
        public float Value { get; }
        public SkillApi Source { get; }
    }
}
