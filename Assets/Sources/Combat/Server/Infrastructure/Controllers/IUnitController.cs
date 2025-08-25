using UnityEngine;

namespace Server.Combat.Infrastructure.Controllers
{
    public interface IUnitController
    {
        //void SetAction(int actionSlot, object? action);

        void TryMove(Vector3 position, float rotation, Vector3 velocity);

        void PerformAction(int slot);

        void HandleHit(DaeHitbox.HitEvent @event);
    }
}
