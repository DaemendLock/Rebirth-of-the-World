using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Presentation.Factories
{
    public interface IHitboxIniter
    {
        void CreateHitboxes(EntityId owner, Transform target);
    }
}
