using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Application.ValueObjects;

using System;

namespace Combat.Domain.Entities
{
    public struct UnitContext
    {
        public HealthOwner Health;
        public ComposureOwner Composure;
        public LifeStateOwner LifeState;
        public Actor Actor;
        public AttributesOwner Attributes;
        public ResourceOwner Resources;
        public SkillOwner Skills;
        public StatusOwner Statuses;
        public ItemOwner Equipment;

        public UnitContext(UnitId id)
        {
            Id = id;
            Actor = new(id);
            Health = new(id, 0, 0);
            Composure = new(id, 0, 0);
            LifeState = new(id);
            Attributes = new(id, new AttributeValue[AttributesOwner.AttributeCount]);
            Resources = new(id, Array.Empty<ResourceValue>());
            Skills = new(id);
            Statuses = new(id);
            Equipment = new(id, new ItemSlot?[Enum.GetValues(typeof(EquipmentSlotType)).Length]);
        }

        public UnitId Id { get; }
    }
}
