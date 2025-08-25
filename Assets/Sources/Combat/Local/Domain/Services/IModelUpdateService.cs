using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.Services
{
    public interface IModelUpdateService
    {
        void RegisterHit(HitboxId source, HurtboxId target, Vector3 position);
        void Update(float deltaTime);
    }
}
