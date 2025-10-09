using Combat.Common.ValueObjects;
using Combat.Local.Data.Entities;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Data.Databases
{
    public class CharacterModelProvider
    {
        private readonly Dictionary<ModelName, GameObject> _values;

        public CharacterModelProvider()
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
