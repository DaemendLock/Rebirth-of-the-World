using Combat.Common.ValueObjects;
using Combat.Local.Controllers;
using Combat.Local.Data.Databases;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Factories;

using Data.Entities.Components;

using UnityEngine;

namespace Testing.Local.Temp.Factories
{
    public class CharacterViewFactory : ICharacterViewFactory
    {
        private readonly HitController _hitController;
        private readonly CharacterModelProvider _characterModelProvider;

        public CharacterViewFactory(CharacterModelProvider characterModelRepository, HitController hitController)
        {
            _characterModelProvider = characterModelRepository;
            _hitController = hitController;
        }

        public CharacterView Create(EntityId id, ModelName modelName)
        {
            GameObject prefab = _characterModelProvider.Get(modelName);

            if (prefab == null)
            {
                throw new System.InvalidOperationException();
            }

            GameObject gameObject = Object.Instantiate(prefab);
            CharacterView result = gameObject.GetComponent<CharacterView>() ?? gameObject.AddComponent<CharacterView>();
            return result;
        }

        public void Init(CharacterView target)
        {
            EntityId entityId = target.Id;

            Hitbox[] hitboxes = target.GetComponentsInChildren<Hitbox>();
            Hurtbox[] hurtboxes = target.GetComponentsInChildren<Hurtbox>();

            foreach (Hitbox hitboxData in hitboxes)
            {
                Collider collider = hitboxData.GetComponent<Collider>();
                _hitController.CreateHitbox(collider, hitboxData.Type, entityId);
                Object.Destroy(hitboxData);
            }

            foreach (Hurtbox hurtboxData in hurtboxes)
            {
                Collider collider = hurtboxData.GetComponent<Collider>();
                _hitController.CreateHurtbox(collider, hurtboxData.Type, entityId);
                Object.Destroy(hurtboxData);
            }
        }
    }
}
