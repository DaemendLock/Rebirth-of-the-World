using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Domain.Repositories
{
    public interface IPositionableRepository
    {
        void Create(Positionable positionable);
        void Update(Positionable positionable);
        Positionable Get(UnitId entityId);
        void Delete(UnitId entityId);

        IReadOnlyCollection<UnitId> FindInRadius(Vector3 center, float radius);
        IReadOnlyCollection<UnitId> FindInCone(Vector3 origin, Quaternion direction, float angle, float maxDistance);
    }
}
