using Combat.Common.ValueObjects;

using Data.Entities;

using UnityEngine;

namespace Combat.Local.Gateways.DataSources
{
    public interface ICharacterPrefabDataSource
    {
        GameObject Get(ModelName model);
        void Register(CharacterModel value);
    }
}
