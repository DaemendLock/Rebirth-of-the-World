using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Local.Application.Queries;
using Combat.Local.Application.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Factories;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class PositionableRepository : IPositionableRepository, IUnitSpatialQuery
    {
        private readonly ISceneObjectDataSource _sceneObjectDataSource;
        private readonly CharacterModelFactory _characterModelFactory;

        public PositionableRepository(ISceneObjectDataSource sceneObjectDataSource, CharacterModelFactory characterModelFactory)
        {
            _sceneObjectDataSource = sceneObjectDataSource;
            _characterModelFactory = characterModelFactory;
        }

        public void Create(Positionable value)
        {
            if (_sceneObjectDataSource.TryGet(value.Id, out Transform parent))
            {
                throw new System.InvalidOperationException();
            }

            parent = _characterModelFactory.Create(parent, value.ModelName);
            _sceneObjectDataSource.Register(value.Id, parent);
        }

        public void Delete(UnitId id) => _sceneObjectDataSource.Destroy(id);

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
    }
}
