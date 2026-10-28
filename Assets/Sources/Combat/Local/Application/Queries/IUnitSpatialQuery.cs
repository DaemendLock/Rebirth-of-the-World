using Combat.Common.Primitives;
using Combat.Domain.Events;
using Combat.Local.Application.ValueObjects;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Application.Queries
{
    public interface IUnitSpatialQuery
    {
        IReadOnlyCollection<UnitId> FindInRadius(Vector3 center, float radius);
        IReadOnlyCollection<UnitId> FindInCone(Vector3 origin, Quaternion direction, float angle, float maxDistance);

        (Vector3 Position, Quaternion Rotation) GetPositionAndRotation(UnitId target);
    }

    public interface IHitRecordQueue
    {
        void Enqueue(HitEvent record);
        bool TryDequeue(out HitEvent record);
    }

    public interface IInputQueue
    {
        void Enqueue(IInputCommand value);
        bool TryDequeue(out IInputCommand record);
    }
}
