using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

using System;

using UnityEngine;

namespace Combat.Local.Application.DTO
{
    public readonly ref struct UnitCreationInfo
    {
        public UnitCreationInfo(ModelName model, Team team, Vector3 position,
            float defaultHealth, float defaultComposure,
            float baseMovespeed, ReadOnlySpan<ResourceValue> defaultResources, ReadOnlySpan<AttributeValue> defaultAttributes,
            ReadOnlySpan<SkillId> skills, float initialHealth = -1f, float initialComposure = -1f)
        {
            Model = model;
            Team = team;
            Position = position;
            InitialHealth = initialHealth;
            DefaultHealth = defaultHealth;
            InitialComposure = initialComposure;
            DefaultComposure = defaultComposure;
            BaseMovespeed = baseMovespeed;
            DefaultResources = defaultResources;
            DefaultAttributes = defaultAttributes;
            Skills = skills;
        }

        public ModelName Model { get; }
        public Team Team { get; }
        public Vector3 Position { get; }
        public float InitialHealth { get; }
        public float DefaultHealth { get; }
        public float InitialComposure { get; }
        public float DefaultComposure { get; }
        public float BaseMovespeed { get; }
        public ReadOnlySpan<ResourceValue> DefaultResources { get; }
        public ReadOnlySpan<AttributeValue> DefaultAttributes { get; }
        public ReadOnlySpan<SkillId> Skills { get; }
    }
}
