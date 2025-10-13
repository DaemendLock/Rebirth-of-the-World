using Combat.Common.ValueObjects;

namespace Combat.API.DTO
{
    public readonly ref struct ResourceChangeRecord
    {
        public readonly ResourceId Resource;
        public readonly SkillApi Souce;
        public readonly float Value;

        public ResourceChangeRecord(ResourceId resource, SkillApi souce, float value)
        {
            Resource = resource;
            Souce = souce;
            Value = value;
        }
    }
}
