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
        public float? BaseComposure;
        public float? InitialComposure;
        public Span<AttributeValue> Attributes;

        public CreateUnitInfo(ModelName modelName, Team team, Vector3 position, float baseHealth, Span<AttributeValue> attributes,
            float baseComposure = 100f, float? initialComposure = null)
        {
            ModelName = modelName;
            Team = team;
            Position = position;
            BaseHealth = baseHealth;
            BaseComposure = baseComposure;
            InitialComposure = initialComposure;
            Attributes = attributes;
        }
    }
}
