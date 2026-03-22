using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Gateways.Repositories.Unit
{
    public sealed class PositionableRepository : IPositionableRepository
    {
        private readonly ISceneCharacterModelDataSource _sceneObjectDataSource;

        public PositionableRepository(ISceneCharacterModelDataSource sceneObjectDataSource)
        {
            _sceneObjectDataSource = sceneObjectDataSource;
        }

        public void Create(Positionable value, Transform parent)
        {
            CharacterModel characterModel = _sceneObjectDataSource.Create(value.Id, value.ModelName, parent);
            characterModel.LookDirection = value.LookDirection;
            characterModel.transform.SetPositionAndRotation(value.Position, value.Rotation);
            characterModel.transform.localScale = value.Scale * Vector3.one;
        }

        public void Delete(EntityId id) => _sceneObjectDataSource.Destroy(id);

        public Positionable Get(EntityId id)
        {
            if (_sceneObjectDataSource.TryGetCharacterModel(id, out var value) == false)
            {
                return default;
            }

            return new(id, value.transform.position, value.transform.rotation, value.transform.localScale.x, value.LookDirection, value.ModelName);
        }

        public void Update(Positionable value)
        {
            if (_sceneObjectDataSource.TryGetCharacterModel(value.Id, out var model) == false)
            {
                return;
            }

            //if(model.ModelName != value.ModelName)
            //{
            //    model = _sceneObjectDataSource.Create(value.Id, value.ModelName, null);
            //}

            model.transform.position = value.Position;
            model.transform.rotation = value.Rotation;
            model.transform.localScale = value.Scale * Vector3.one;
        }

        public ICollection<EntityId> FindInRadius(Vector3 center, float radius)
        {
            return _sceneObjectDataSource.FindCharacterInRadius(center, radius);
        }
    }
}
