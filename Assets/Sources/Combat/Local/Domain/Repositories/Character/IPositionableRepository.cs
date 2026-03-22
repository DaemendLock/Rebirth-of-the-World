using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Domain.Repositories
{
    public interface IPositionableRepository
    {
        void Create(Positionable positionable, Transform parent);
        void Update(Positionable positionable);
        Positionable Get(EntityId entityId);
        void Delete(EntityId entityId);

        ICollection<EntityId> FindInRadius(Vector3 center, float radius);
    }
}
