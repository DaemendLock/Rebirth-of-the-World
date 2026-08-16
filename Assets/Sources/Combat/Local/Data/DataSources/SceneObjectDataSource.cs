using Combat.Common.Primitives;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using Data.Levels;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Combat.Local.Data.DataSources
{
    public sealed class SceneObjectDataSource : ISceneObjectDataSource
    {
        private readonly Dictionary<UnitId, Transform> _values;

        private readonly Collider[] _buffer;

        public SceneObjectDataSource()
        {
            _values = new();

            _buffer = new Collider[128];
        }

        public Scene? Scene { get; set; }

        public void Register(UnitId entityId, Transform transform)
        {
            _values[entityId] = transform;
        }

        public Transform GetOrCreate(UnitId id)
        {
            if (_values.TryGetValue(id, out Transform value) == false)
            {
                value = new GameObject(id.ToString()).transform;
                _values[id] = value;
            }

            return value;
        }

        public void Destroy(UnitId id)
        {
            if (_values.Remove(id, out Transform value) == false)
            {
                return;
            }

            UnityEngine.Object.Destroy(value.gameObject);
        }

        public bool TryGet(UnitId id, out Transform model) => _values.TryGetValue(id, out model);

        public bool TryGetValue(UnitId entityId, out Transform result) => _values.TryGetValue(entityId, out result);

        public IReadOnlyCollection<UnitId> FindCharactersInRadius(Vector3 origin, float radius)
        {
            int count = Physics.OverlapSphereNonAlloc(origin, radius, _buffer, LayerMask.GetMask("Units"));
            List<UnitId> result = new(count);

            for (int i = 0; i < count; i++)
            {
                if (_buffer[i].attachedRigidbody.TryGetComponent(out CharacterModelComponent view))
                {
                    result.Add(view.Id);
                }
            }

            return result;
        }

        public int FindCharactersInRadiusNonAlloc(Vector3 position, float radius, Span<UnitId> buffer)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyCollection<UnitId> FindCharactersInCone(Vector3 origin, Quaternion direction, float angle, float maxDistance)
        {
            int count = Physics.OverlapSphereNonAlloc(origin, maxDistance, _buffer, LayerMask.GetMask("Units"));
            List<UnitId> result = new(count);

            for (int i = 0; i < count; i++)
            {
                if (_buffer[i].attachedRigidbody.TryGetComponent(out CharacterModelComponent view))
                {
                    result.Add(view.Id);
                }
            }

            return result;
        }

        public SpawnpointModel[] GetSpawnpoints()
        {
            if (Scene == null)
            {
                return Array.Empty<SpawnpointModel>();
            }

            var items = UnityEngine.Object.FindObjectsByType<PlayerSpawnpoint>(FindObjectsSortMode.None);
            return items.Select(value => new SpawnpointModel(value.transform.position, value.transform.rotation)).ToArray();
        }
    }
}
