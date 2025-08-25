using UnityEngine;

namespace Server.Combat.Domain.Statuses.StatusEffects
{
    public interface IMovementModifier
    {
        Vector3 GetVelocityModification();
    }
}
