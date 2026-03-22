using Combat.Common.ValueObjects;
using Combat.Local.Data.Databases;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Presenters;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Data.Presentation
{
    public class CharacterModelFactory : ICharacterModelFactory
    {
        private readonly CharacterModelProvider _characterModelProvider;

        public CharacterModelFactory(CharacterModelProvider characterModelProvider)
        {
            _characterModelProvider = characterModelProvider;
        }

        public CharacterModel Create(ModelName name, Transform parent)
        {
            if (parent == null)
            {
                parent = CreatePrefab(name);
            }

            CharacterModel result = parent.gameObject.AddComponent<CharacterModel>();
            result.ModelName = name;
            return result;
        }

        private Transform CreatePrefab(ModelName modelName)
        {
            GameObject prefab = _characterModelProvider.Get(modelName);

            if (prefab == null)
            {
                throw new System.InvalidOperationException();
            }

            GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
            return gameObject.transform;
        }
    }

    public interface ICharacterModelFactory
    {
        CharacterModel Create(ModelName name, Transform parent);
    }

    public class SceneCharacterModelDataSource : ICharacterViewContainer, ISceneObjectDataSource
    {
        private readonly Dictionary<EntityId, CharacterModel> _values;
        private readonly ICharacterModelFactory _factory;

        public SceneCharacterModelDataSource(ICharacterModelFactory factory)
        {
            _values = new();
            _factory = factory;
        }

        public CharacterModel Create(EntityId id, ModelName name, Transform parent)
        {
            CharacterModel result = _factory.Create(name, parent);
            _values[id] = result;
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

        public CharacterModel GetCharacterModel(EntityId id) => _values[id];

        public bool TryGetCharacterModel(EntityId id, out CharacterModel model) => _values.TryGetValue(id, out model);

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
