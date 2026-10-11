using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

using System;

using UnityEngine;

namespace Combat.API.DTO
{
    public ref struct CreateUnitInfo
    {
        public ModelName ModelName;
        public Team Team;
        public Vector3 Position;
        public float BaseHealth;
        public Span<AttributeValue> Attributes;

        public CreateUnitInfo(ModelName modelName, Team team, Vector3 position, float baseHealth, Span<AttributeValue> attributes)
        {
            ModelName = modelName;
            Team = team;
            Position = position;
            BaseHealth = baseHealth;
            Attributes = attributes;
        }
    }
}
