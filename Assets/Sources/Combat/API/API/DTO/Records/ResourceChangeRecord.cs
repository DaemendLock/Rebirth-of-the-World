using Combat.Common.ValueObjects;

namespace Combat.API.DTO
{
    public readonly ref struct ResourceChangeRecord
    {
        public readonly ResourceId Resource;
        public readonly AbilityApi Souce;
        public readonly float Value;

        public ResourceChangeRecord(ResourceId resource, AbilityApi souce, float value)
        {
            Resource = resource;
            Souce = souce;
            Value = value;
        }
    }
}
