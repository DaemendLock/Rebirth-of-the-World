using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Domain.Repositories
{
    public interface IPositionableRepository
    {
        void Create(Positionable positionable);
        void Update(Positionable positionable);
        Positionable Get(EntityId entityId);
        void Delete(EntityId entityId);

        ICollection<EntityId> FindInRadius(Vector3 origin, float radius);

        int FindInRadiusNoAlloc(Vector3 origin, float radius, Span<EntityId> buffer);
    }
}
