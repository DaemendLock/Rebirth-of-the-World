using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.OldAttributes;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Infrastructure.Controllers;

using UnityEngine;

namespace Combat.Local.Factories
{
    public interface IUnitControllerFactory
    {
        public readonly ref struct UnitModelCreationData
        {
            public readonly Transform Parent;
            public readonly ModelName UnitId;
            public readonly Vector3 Position;
            public readonly Team Team;
            public readonly float BaseHealth;
            public readonly float CurrentHealth;
            public readonly IAttributeCollection<Attribute> DefaultAttributes;
            public readonly IEnumerable<SkillId> Skills;

            public UnitModelCreationData(ModelName unitId, byte team, Transform parent, Vector3 position, float baseHealth, float currentHealth, IAttributeCollection<Attribute> defaultAttributes, IEnumerable<SkillId> skills)
            {
                UnitId = unitId;
                Parent = parent;
                Position = position;
                Team = new(team);
                BaseHealth = baseHealth;
                CurrentHealth = currentHealth;
                DefaultAttributes = defaultAttributes;
                Skills = skills;
            }
        }

        UnitController Create(UnitModelCreationData context);
    }
}
