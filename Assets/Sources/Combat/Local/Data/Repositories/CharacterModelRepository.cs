using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Data.Entities;

using UnityEngine;

namespace Temp.Repositories.Implementations
{
    public class CharacterModelRepository
    {
        private readonly Dictionary<ModelName, GameObject> _values;

        public CharacterModelRepository()
        {
            _values = new();

            foreach(CharacterModel model in Resources.LoadAll<CharacterModel>("Temp/TestCharacterModels"))
            {
                _values.Add(model.Name, model.Prefab);
            }
        }

        public GameObject Get(ModelName model) => _values.GetValueOrDefault(model, null);
    }
}
