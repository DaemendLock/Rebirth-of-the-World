namespace Server.Combat.Domain.DTO
{
    public readonly struct ResourceValue
    {
        public readonly ResourceType ResourceType;
        public readonly float Value;

        public ResourceValue(ResourceType resourceType, float value)
        {
            ResourceType = resourceType;
            Value = value;
        }
    }
}