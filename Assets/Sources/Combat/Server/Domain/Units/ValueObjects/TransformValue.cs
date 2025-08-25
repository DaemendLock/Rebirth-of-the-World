using UnityEngine;

namespace Server.Combat.Domain.Units.ValueObjects
{
    public struct TransformValue
    {
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public Vector3 Velocity { get; set; }
    }
}
