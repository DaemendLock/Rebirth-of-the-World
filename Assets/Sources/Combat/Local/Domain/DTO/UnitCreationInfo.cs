using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System;

using UnityEngine;

namespace Combat.Local.Domain.DTO
{
    public readonly ref struct UnitCreationInfo
    {
        public UnitCreationInfo(ModelName model, Team team, Vector3 position,
            float initialHealth, float defaultHealth,
            ReadOnlySpan<AttributeValue> defaultAttributes, ReadOnlySpan<ResourceValue> defaultResources, ReadOnlySpan<SkillId> skills)
        {
            Model = model;
            Team = team;
            Position = position;
            InitialHealth = initialHealth;
            DefaultHealth = defaultHealth;
            DefaultResources = defaultResources;
            DefaultAttributes = defaultAttributes;
            Skills = skills;
        }

        public ModelName Model { get; }
        public Team Team { get; }
        public Vector3 Position { get; }
        public float InitialHealth { get; }
        public float DefaultHealth { get; }
        public ReadOnlySpan<ResourceValue> DefaultResources { get; }
        public ReadOnlySpan<AttributeValue> DefaultAttributes { get; }
        public ReadOnlySpan<SkillId> Skills { get; }
    }
}
