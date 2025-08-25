using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

using UnityEngine;

namespace Combat.Local.Domain.Entities
{
    public interface IAction
    {
        EntityId Actor { get; }
        bool IsActive { get; }
        float ActiveTime { get; set; }

        bool AllowMovement { get; }
        void Start();
        void Update();
        bool HandleHit(Hitbox hitbox, Hurtbox hurtbox, Vector3 position);
    }
}
