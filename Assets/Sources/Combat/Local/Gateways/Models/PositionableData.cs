using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    public readonly struct PositionableData
    {
        public PositionableData(ModelName model, Quaternion lookDirection)
        {
            Model = model;
            LookDirection = lookDirection;
        }

        public ModelName Model { get; }
        public Quaternion LookDirection { get; }
    }
}
