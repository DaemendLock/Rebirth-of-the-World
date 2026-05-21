using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Gateways.Repositories.Characters
{
    public class MovementEffectOwnerRepository : IMovementEffectOwnerRepository
    {
        private readonly Dictionary<UnitId, MovementEffectComponent> _values;
        private readonly ISceneObjectDataSource _characterModelDataSource;

        public MovementEffectOwnerRepository(ISceneObjectDataSource characterModelDataSource)
        {
            _values = new();
            _characterModelDataSource = characterModelDataSource;
        }

        public void Create(MovementEffectOwner value)
        {
            if (_values.ContainsKey(value.Id))
            {
                throw new System.InvalidOperationException($"Key {value.Id} already exists");
            }

            Transform transform = _characterModelDataSource.GetOrCreate(value.Id);
            _values[value.Id] = transform.gameObject.AddComponent<MovementEffectComponent>();
        }

        public void Delete(UnitId target)
        {
            if (_values.TryGetValue(target, out MovementEffectComponent model) == false)
            {
                return;
            }

            UnityEngine.Object.Destroy(model);
            _values.Remove(target);
        }

        public MovementEffectOwner Get(UnitId id)
        {
            if (_values.TryGetValue(id, out MovementEffectComponent model) == false)
            {
                throw new System.InvalidOperationException($"Key {id} does not exists");
            }

            return new(id, model.Effects);
        }
    }
}
