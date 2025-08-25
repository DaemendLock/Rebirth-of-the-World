using DaeHitbox;

using UnityEngine;

namespace Client.Combat.Presentation.Implementations.Units.Components
{
    public class Hitbox : DaeHitbox.Components.Hitbox
    {
        [field: SerializeField] public HitboxType Type { get; private set; }

        protected override bool TryHandleCollsition(Collider other, out HitEvent hitEvent)
        {
            if (!other.TryGetComponent(out Hurtbox hurtbox))
            {
                hitEvent = default;
                return false;
            }

            hitEvent = new(this, hurtbox);
            return true;
        }
    }
}
