using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Local.Application.Queries;
using Combat.Local.Application.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Factories;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

using UnityEngine;

using HitboxInfo = Data.Entities.Components.Hitbox;
using HurtboxInfo = Data.Entities.Components.Hurtbox;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class PositionableRepository : IPositionableRepository, IUnitSpatialQuery
    {
        private readonly ISceneObjectDataSource _sceneObjectDataSource;
        private readonly CharacterModelFactory _characterModelFactory;
        private readonly IHitRecordQueue _hitRecordQueue;
        private readonly Dictionary<UnitId, List<HitboxModelComponent>> _hitboxes;
        private readonly Dictionary<UnitId, List<HurtboxModelComponent>> _hurtboxes;

        public PositionableRepository(ISceneObjectDataSource sceneObjectDataSource, CharacterModelFactory characterModelFactory, IHitRecordQueue hitRecordQueue)
        {
            _sceneObjectDataSource = sceneObjectDataSource;
            _characterModelFactory = characterModelFactory;
            _hitRecordQueue = hitRecordQueue;

            _hitboxes = new();
            _hurtboxes = new();
        }

        public void Create(Positionable value)
        {
            if (_sceneObjectDataSource.TryGet(value.Id, out Transform parent) == false)
            {
                parent = _characterModelFactory.Create(parent, value.ModelName);
                _sceneObjectDataSource.Register(value.Id, parent);
            }

            parent.SetPositionAndRotation(value.Position, value.Rotation);
            _hitboxes.Add(value.Id, CreateHitboxes(value.Id, parent));
            _hurtboxes.Add(value.Id, CreateHurtboxes(value.Id, parent));
        }

        public void Delete(UnitId id)
        {
            DeleteHitboxes(id);
            DeleteHurtboxes(id);
            _sceneObjectDataSource.Destroy(id);
        }

        public IReadOnlyCollection<UnitId> FindInRadius(Vector3 center, float radius)
        {
            Collider[] values = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Units"));
            List<UnitId> result = new(values.Length);

            foreach (Collider value in values)
            {
                if (value.attachedRigidbody.TryGetComponent(out CharacterModelComponent view))
                {
                    result.Add(view.Id);
                }
            }

            return result;
        }

        public IReadOnlyCollection<UnitId> FindInCone(Vector3 origin, Quaternion direction, float angle, float maxDistance) =>
            _sceneObjectDataSource.FindCharactersInCone(origin, direction, angle, maxDistance);

        public (Vector3, Quaternion) GetPositionAndRotation(UnitId target)
        {
            if (_sceneObjectDataSource.TryGet(target, out var model) == false || model == null)
            {
                throw new KeyNotFoundException();
            }

            return (model.position, model.rotation);
        }

        private List<HitboxModelComponent> CreateHitboxes(UnitId id, Transform root)
        {
            HitboxInfo[] hitboxes = root.GetComponentsInChildren<HitboxInfo>();

            List<HitboxModelComponent> result = new(hitboxes.Length);

            foreach (HitboxInfo hitboxData in hitboxes)
            {
                HitboxModelComponent value = hitboxData.gameObject.AddComponent<HitboxModelComponent>();
                value.Owner = id;
                value.Type = hitboxData.Type;
                value.Hitted += _hitRecordQueue.Enqueue;
                result.Add(value);

                UnityEngine.Object.Destroy(hitboxData);
            }

            return result;
        }

        private void DeleteHitboxes(UnitId id)
        {
            if (_hitboxes.TryGetValue(id, out var models) == false)
            {
                return;
            }

            foreach (HitboxModelComponent value in models)
            {
                UnityEngine.Object.Destroy(value);
            }

            _hitboxes.Remove(id);
        }

        private List<HurtboxModelComponent> CreateHurtboxes(UnitId id, Transform root)
        {
            HurtboxInfo[] hurtboxes = root.GetComponentsInChildren<HurtboxInfo>();
            List<HurtboxModelComponent> result = new();

            foreach (HurtboxInfo hurtboxData in hurtboxes)
            {
                var value = hurtboxData.gameObject.AddComponent<HurtboxModelComponent>();
                value.Type = hurtboxData.Type;
                value.Owner = id;
                result.Add(value);

                UnityEngine.Object.Destroy(hurtboxData);
            }

            return result;
        }

        private void DeleteHurtboxes(UnitId id)
        {
            if (_hurtboxes.TryGetValue(id, out var models) == false)
            {
                return;
            }

            foreach (var value in models)
            {
                UnityEngine.Object.Destroy(value);
            }

            _hurtboxes.Remove(id);
        }
    }
}
