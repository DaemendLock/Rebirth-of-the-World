using Combat.Common.ValueObjects;
using Combat.Local.Data.Databases;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Factories;

using UnityEngine;

namespace Combat.Local.Data.Factories
{
    public class CharacterViewFactory : ICharacterViewFactory
    {
        private readonly CharacterModelProvider _characterModelProvider;

        public CharacterViewFactory(CharacterModelProvider characterModelRepository)
        {
            _characterModelProvider = characterModelRepository;
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
            result.Id = id;
            gameObject.name = modelName.ToString() + id.ToString();

            return result;
        }
    }
}
