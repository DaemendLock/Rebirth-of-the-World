using Combat.Common.Primitives;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Combat.Local.Gateways.DataSources
{
    public readonly struct SpawnpointModel
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;

        public SpawnpointModel(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }

    public interface ISceneObjectDataSource
    {
        Scene? Scene { get; set; }

        Transform GetOrCreate(UnitId id);
        void Destroy(UnitId id);
        bool TryGet(UnitId id, out Transform model);

        IReadOnlyCollection<UnitId> FindCharactersInRadius(Vector3 location, float radius);

        IReadOnlyCollection<UnitId> FindCharactersInCone(Vector3 origin, Quaternion direction, float angle, float maxDistance);
        void Register(UnitId entityId, Transform transform);

        SpawnpointModel[] GetSpawnpoints();
    }
}
