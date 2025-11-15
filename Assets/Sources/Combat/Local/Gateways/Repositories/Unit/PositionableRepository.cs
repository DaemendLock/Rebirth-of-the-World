using Combat.Common.ValueObjects;
using Combat.Local.Data.Models;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Gateways.Repositories.Unit
{
    public sealed class PositionableRepository : IPositionableRepository
    {
        private readonly ISceneObjectDataSource _sceneObjectDataSource;
        private readonly Dictionary<EntityId, PositionableData> _values;

        public PositionableRepository(ISceneObjectDataSource sceneObjectDataSource)
        {
            _sceneObjectDataSource = sceneObjectDataSource;

            _values = new();
        }

        public void Create(Positionable value)
        {
            PositionableData data = new(value.ModelName, value.LookDirection);

            _values.Add(value.Id, data);

            if (_sceneObjectDataSource.TryGetCharacterTransform(value.Id, out Transform transform) == false)
            {
                return;
            }

            transform.SetPositionAndRotation(value.Position, value.Rotation);
            transform.localScale = value.Scale * Vector3.one;
        }

        public void Delete(EntityId id) => _values.Remove(id);

        public Positionable Get(EntityId id)
        {
            if (_values.TryGetValue(id, out var value) == false)
            {
                return default;
            }

            if (_sceneObjectDataSource.TryGetCharacterTransform(id, out Transform transform) == false)
            {
                return new(id, default, default, 1f, value.LookDirection, value.Model);
            }

            return new(id, transform.position, transform.rotation, transform.localScale.x, value.LookDirection, value.Model);
        }

        public void Update(Positionable value)
        {
            if (_values.TryGetValue(value.Id, out var currentValue) == false)
            {
                return;
            }

            _values[value.Id] = new(value.ModelName, value.LookDirection);

            if (_sceneObjectDataSource.TryGetCharacterTransform(value.Id, out Transform transform) == false)
            {
                return;
            }

            transform.position = value.Position;
            transform.rotation = value.Rotation;
            transform.localScale = value.Scale * Vector3.one;
        }

        public ICollection<EntityId> FindInRadius(Vector3 origin, float radius)
        {
            return _sceneObjectDataSource.FindCharacterInRadius(origin, radius);
        }

        public int FindInRadiusNoAlloc(Vector3 origin, float radius, Span<EntityId> buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}
