using Combat.Common.ValueObjects;
using Combat.Local.Data.Factories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Presenters;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Data.DataSources
{
    public class SceneCharacterModelDataSource : ICharacterViewContainer, ICharacterModelDataSource
    {
        private readonly Dictionary<EntityId, CharacterModel> _values;
        private readonly CharacterModelFactory _factory;

        public SceneCharacterModelDataSource(CharacterModelFactory factory)
        {
            _factory = factory;
            _values = new();
        }

        public CharacterModel Create(EntityId id, ModelName name, Transform parent)
        {
            CharacterModel result = _factory.Create(name, parent);
            result.Id = id;
            result.name = name.ToString() + id.ToString();
            _values[id] = result;

            _factory.Init(result);
            return result;
        }

        public void Destroy(EntityId id)
        {
            if (_values.TryGetValue(id, out CharacterModel value) == false)
            {
                return;
            }

            UnityEngine.Object.Destroy(value.gameObject);
        }

        public bool TryGetCharacterModel(EntityId id, out CharacterModel model) => _values.TryGetValue(id, out model);

        public bool TryGetMovementContainer(EntityId id, out IMovementEffectContainer result)
        {
            if (TryGetCharacterModel(id, out CharacterModel model) == false)
            {
                result = default;
                return false;
            }

            if (model.TryGetComponent(out MovementEffectComponent component) == false)
            {
                result = default;
                return false;
            }

            result = component;
            return true;
        }

        public bool TryGetValue(EntityId entityId, out Transform result)
        {
            if (_values.TryGetValue(entityId, out CharacterModel model) == false)
            {
                result = default;
                return false;
            }

            result = model.transform;
            return true;
        }

        public ICollection<EntityId> FindCharacterInRadius(Vector3 origin, float radius)
        {
            Collider[] values = Physics.OverlapSphere(origin, radius, LayerMask.GetMask("Units"));
            List<EntityId> result = new(values.Length);

            foreach (Collider collider in values)
            {
                if (collider.attachedRigidbody.TryGetComponent(out CharacterView view))
                {
                    result.Add(view.Id);
                }
            }

            return result;
        }

        public int FindCharactersInRadiusNonAlloc(Vector3 position, float radius, Span<EntityId> buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}
