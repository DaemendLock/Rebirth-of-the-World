using Combat.Common.ValueObjects;

using System;

using UnityEngine;

namespace Combat.Local.Domain.DTO
{
    public readonly ref struct CreateCharacterDTO
    {
        public CreateCharacterDTO(ModelName model, Team team, Vector3 position, float initialHealth, float defaultHealth, Span<AttributeValue> defaultAttributes, Span<SkillId> skills)
        {
            Model = model;
            Team = team;
            Position = position;
            InitialHealth = initialHealth;
            DefaultHealth = defaultHealth;
            DefaultAttributes = defaultAttributes;
            Skills = skills;
        }

        public ModelName Model { get; }
        public Team Team { get; }
        public Vector3 Position { get; }
        public float InitialHealth { get; }
        public float DefaultHealth { get; }
        public Span<AttributeValue> DefaultAttributes { get; }
        public Span<SkillId> Skills { get; }
    }
}
