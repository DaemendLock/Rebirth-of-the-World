using Combat.Common.Primitives;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Domain.Queries
{
    public interface IUnitSpatialQuery
    {
        IReadOnlyCollection<UnitId> FindInRadius(Vector3 center, float radius);
        IReadOnlyCollection<UnitId> FindInCone(Vector3 origin, Quaternion direction, float angle, float maxDistance);
    }
}
