using Combat.Common.ValueObjects;
using Combat.Local.Controllers;
using Combat.Local.Data.Databases;
using Combat.Local.Gateways.Models;

using Data.Entities.Components;

using UnityEngine;

namespace Combat.Local.Data.Factories
{
    public class CharacterModelFactory
    {
        private readonly CharacterModelProvider _characterModelProvider;
        private readonly HitController _hitController;

        public CharacterModelFactory(CharacterModelProvider characterModelProvider, HitController hitController)
        {
            _characterModelProvider = characterModelProvider;
            _hitController = hitController;
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

        public void Init(CharacterModel target)
        {
            EntityId entityId = target.Id;

            Hitbox[] hitboxes = target.GetComponentsInChildren<Hitbox>();
            Hurtbox[] hurtboxes = target.GetComponentsInChildren<Hurtbox>();

            foreach (Hitbox hitboxData in hitboxes)
            {
                Collider collider = hitboxData.GetComponent<Collider>();
                _hitController.CreateHitbox(collider, hitboxData.Type, entityId);
                UnityEngine.Object.Destroy(hitboxData);
            }

            foreach (Hurtbox hurtboxData in hurtboxes)
            {
                Collider collider = hurtboxData.GetComponent<Collider>();
                _hitController.CreateHurtbox(collider, hurtboxData.Type, entityId);
                UnityEngine.Object.Destroy(hurtboxData);
            }
        }
    }
}
