using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.Services
{
    public interface IHitHandlingService
    {
        void HandleHit(HitboxId source, HurtboxId target, Vector3 position);
    }
}
