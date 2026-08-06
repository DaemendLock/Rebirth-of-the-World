using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Factories;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public sealed class PositionableRepository : IPositionableRepository
    {
        private readonly Dictionary<UnitId, CharacterModelComponent> _values;
        private readonly ISceneObjectDataSource _sceneObjectDataSource;
        private readonly CharacterModelFactory _characterModelFactory;

        public PositionableRepository(ISceneObjectDataSource sceneObjectDataSource, CharacterModelFactory characterModelFactory)
        {
            _values = new();
            _sceneObjectDataSource = sceneObjectDataSource;
            _characterModelFactory = characterModelFactory;
        }

        public void Create(Positionable value)
        {
            if (_values.ContainsKey(value.Id))
            {
                return;
            }

            CharacterModelComponent result;

            if (_sceneObjectDataSource.TryGet(value.Id, out Transform parent) == false)
            {
                parent = _characterModelFactory.Create(parent, value.ModelName);
                _sceneObjectDataSource.Register(value.Id, parent);
            }

            result = parent.gameObject.AddComponent<CharacterModelComponent>();

            result.Id = value.Id;
            result.name = value.ModelName.ToString() + value.Id.ToString();
            result.ModelName = value.ModelName;

            result.LookDirection = value.LookDirection;
            result.transform.SetPositionAndRotation(value.Position, value.Rotation);
            result.transform.localScale = value.Scale * Vector3.one;
            result.Velocity = value.Velocity;

            _values.Add(value.Id, result);
        }

        public void Delete(UnitId id) => _sceneObjectDataSource.Destroy(id);

        public Positionable Get(UnitId id)
        {
            if (_values.TryGetValue(id, out CharacterModelComponent model) == false || model == null)
            {
                return default;
            }

            return new(id, model.transform.position, model.transform.rotation, model.transform.localScale.x, model.LookDirection, model.ModelName, model.Velocity);
        }

        public void Update(Positionable value)
        {
            if (_values.TryGetValue(value.Id, out var model) == false)
            {
                return;
            }

            //if(model.ModelName != value.ModelName)
            //{
            //    model = _sceneObjectDataSource.Create(value.Id, value.ModelName, null);
            //}
            model.transform.SetPositionAndRotation(value.Position, value.Rotation);
            model.transform.localScale = value.Scale * Vector3.one;
            model.Velocity = value.Velocity;
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

        public IReadOnlyCollection<UnitId> FindInCone(Vector3 origin, Quaternion direction, float angle, float maxDistance)
        {
            return _sceneObjectDataSource.FindCharactersInCone(origin, direction, angle, maxDistance);
        }
    }
}
