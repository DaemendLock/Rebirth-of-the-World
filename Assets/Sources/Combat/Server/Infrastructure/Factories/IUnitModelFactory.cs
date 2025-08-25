using System.Collections.Generic;

using DaeHitbox;

using Server.Combat.Domain.Attributes;
using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Skills;
using Server.Combat.Domain.Units.ValueObjects;

using UnityEngine;

namespace Server.Combat.Infrastructure.Factories
{
    public interface IUnitModelFactory
    {
        public readonly ref struct UnitModelCreationData
        {
            public readonly UnitId UnitId;
            public readonly Team Team;
            public readonly Vector3 Position;
            public readonly float Rotation;
            public readonly float MaxHealth;
            public readonly float CurrentHealth;
            public readonly IAttributeCollection<Attribute> DefaultAttributes;
            public readonly IEnumerable<SkillId> Skills;
            public readonly IHitboxCollection Hitboxes;

            public UnitModelCreationData(UnitId unitId, byte team, Vector3 position, float rotation, float maxHealth, float currentHealth, IAttributeCollection<Attribute> defaultAttributes, IEnumerable<SkillId> skills, IHitboxCollection hitboxes)
            {
                UnitId = unitId;
                Team = new(team);
                Position = position;
                Rotation = rotation;
                MaxHealth = maxHealth;
                CurrentHealth = currentHealth;
                DefaultAttributes = defaultAttributes;
                Skills = skills;
                Hitboxes = hitboxes;
            }
        }

        Unit Create(UnitModelCreationData context);
    }
}
