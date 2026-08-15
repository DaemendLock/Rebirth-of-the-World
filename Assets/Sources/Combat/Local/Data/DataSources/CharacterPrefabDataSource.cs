using Combat.Common.ValueObjects;
using Combat.Local.Gateways.DataSources;

using Data.Entities;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Data.Databases
{
    public class CharacterPrefabDataSource : ICharacterPrefabDataSource
    {
        private readonly Dictionary<ModelName, GameObject> _values;

        public CharacterPrefabDataSource()
        {
            _values = new();

            foreach (CharacterModel model in Resources.LoadAll<CharacterModel>("Temp/TestCharacterModels"))
            {
                Register(model);
            }
        }

        public void Register(CharacterModel value) => _values.Add(value.Name, value.Prefab);

        public GameObject Get(ModelName model) => _values.GetValueOrDefault(model, null);
    }
}
