using Combat.Common.Primitives;
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

            if (model == null)
            {
                _values.Remove(id);
                throw new System.InvalidOperationException($"Key {id} does not exists");
            }

            return new(id, model.Effects);
        }
    }

    public sealed class ScaleEffectOwnerRepository : IScaleEffectOwnerRepository
    {
        private readonly Dictionary<UnitId, ScaleEffectComponent> _values;
        private readonly ISceneObjectDataSource _characterModelDataSource;

        public ScaleEffectOwnerRepository(ISceneObjectDataSource characterModelDataSource)
        {
            _characterModelDataSource = characterModelDataSource;
            _values = new();
        }

        public void Create(ScaleEffectOwner value)
        {
            if (_values.ContainsKey(value.Id))
            {
                throw new System.InvalidOperationException($"Key {value.Id} already exists");
            }

            Transform transform = _characterModelDataSource.GetOrCreate(value.Id);
            ScaleEffectComponent model = transform.gameObject.AddComponent<ScaleEffectComponent>();
            model.Values = value.Values.ToArray();
            _values[value.Id] = model;
        }

        public ScaleEffectOwner Get(UnitId id)
        {
            if (_values.TryGetValue(id, out var model) == false)
            {
                throw new System.InvalidOperationException($"Key {id} does not exists");
            }

            if (model == null)
            {
                _values.Remove(id);
                throw new System.InvalidOperationException($"Key {id} does not exists");
            }

            return new(id, model.Values);
        }

        public void Update(ScaleEffectOwner value)
        {
            if (_values.TryGetValue(value.Id, out var model) == false)
            {
                throw new System.InvalidOperationException($"Key {value.Id} does not exists");
            }

            if (model == null)
            {
                _values.Remove(value.Id);
                return;
            }

            model.Values = value.Values.ToArray();
        }

        public void Delete(UnitId target)
        {
            if (_values.TryGetValue(target, out var model) == false)
            {
                return;
            }

            UnityEngine.Object.Destroy(model);
            _values.Remove(target);
        }
    }
}
