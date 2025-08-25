using UnityEngine;

namespace Client.Combat.Domain.Units.Components
{
    public interface IPositionOwner
    {
        Vector3 Position { get; set; }
        Vector3 Velocity { get; set; }
        float Rotation { get; set; }
        float Scale { get; set; }
    }
}
