using Combat.Common.ValueObjects;
using Combat.Local.Gateways.DataSources;

using UnityEngine;

namespace Combat.Local.Gateways.Factories
{
    public class CharacterModelFactory
    {
        private readonly ICharacterPrefabDataSource _characterModelProvider;

        public CharacterModelFactory(ICharacterPrefabDataSource characterModelProvider)
        {
            _characterModelProvider = characterModelProvider;
        }

        public Transform Create(Transform parent, ModelName name)
        {
            GameObject prefab = _characterModelProvider.Get(name);

            if (prefab == null)
            {
                throw new System.InvalidOperationException();
            }

            GameObject gameObject = UnityEngine.Object.Instantiate(prefab, parent);
            return gameObject.transform;
        }
    }
}
