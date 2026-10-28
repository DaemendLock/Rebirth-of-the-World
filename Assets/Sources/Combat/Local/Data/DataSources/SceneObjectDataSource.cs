using Combat.Common.Primitives;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Factories;
using Combat.Local.Gateways.Models;
using Combat.Local.Presentation.Providers;
using Combat.Local.Presentation.Views;

using Data.Levels;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

using HitboxInfo = Data.Entities.Components.Hitbox;

namespace Combat.Local.Data.DataSources
{
    public sealed class Idk
    {
        private readonly Dictionary<UnitId, Transform> _values;
        private readonly CharacterModelFactory _characterModelFactory;

        public Transform Create(UnitId id, ModelName modelName)
        {
            Transform model = _characterModelFactory.Create(null, modelName);
            _values[id] = model;
            
            var hitboxes = CreateHitboxes(id, model);

            foreach(var item in hitboxes)
            {
                //item.Hitted += 
            }

            return model;
        }

        public List<HitboxModelComponent> CreateHitboxes(UnitId id, Transform model)
        {
            HitboxInfo[] hitboxes = model.GetComponentsInChildren<HitboxInfo>();

            List<HitboxModelComponent> result = new(hitboxes.Length);

            foreach (HitboxInfo hitboxData in hitboxes)
            {
                HitboxModelComponent value = hitboxData.gameObject.AddComponent<HitboxModelComponent>();
                value.Owner = id;
                value.Type = hitboxData.Type;
                result.Add(value);

                UnityEngine.Object.Destroy(hitboxData);
            }

            return result;
        }
    }

    public sealed class SceneObjectDataSource : ISceneObjectDataSource, ITransformProvider
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

        public void Destroy(UnitId id)
        {
            if (_values.Remove(id, out Transform value) == false)
            {
                return;
            }

            UnityEngine.Object.Destroy(value.gameObject);
        }

        public bool TryGet(UnitId id, out Transform model) => _values.TryGetValue(id, out model) && model != null;

        public IReadOnlyCollection<UnitId> FindCharactersInRadius(Vector3 origin, float radius)
        {
            int count = Physics.OverlapSphereNonAlloc(origin, radius, _buffer, LayerMask.GetMask("Units"));
            List<UnitId> result = new(count);

            for (int i = 0; i < count; i++)
            {
                if (_buffer[i].attachedRigidbody.TryGetComponent(out CharacterView view))
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
