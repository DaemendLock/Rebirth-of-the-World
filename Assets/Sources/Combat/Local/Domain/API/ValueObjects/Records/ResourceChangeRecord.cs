using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.API.Statuses
{
    public readonly ref struct ResourceChangeRecord
    {
        public readonly ResourceId Resource;
        public readonly ScriptedSkill Souce;
        public readonly float Value;

        public ResourceChangeRecord(ResourceId resource, ScriptedSkill souce, float value)
        {
            Resource = resource;
            Souce = souce;
            Value = value;
        }
    }
}
