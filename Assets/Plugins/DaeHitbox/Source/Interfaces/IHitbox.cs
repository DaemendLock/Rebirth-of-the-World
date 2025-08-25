using System;

namespace DaeHitbox
{
    public enum HitboxType
    {
        None,
        Weapon,
        RightFoot,
        LeftFoot,
    }

    public readonly struct HitEvent
    {
        public readonly IHitbox hitbox;
        public readonly IHurtbox Hurtbox;

        public HitEvent(IHitbox hitbox, IHurtbox hurtbox)
        {
            this.hitbox = hitbox;
            Hurtbox = hurtbox;
        }
    }

    public interface IHitbox
    {
        event Action<HitEvent> Hitted;

        HitboxType HitboxType { get; }

        void SetActive(bool active);
    }
}