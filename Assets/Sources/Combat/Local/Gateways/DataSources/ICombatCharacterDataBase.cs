using Combat.Common.Primitives;
using Combat.Local.Gateways.Models;

using UnityEngine;

namespace Combat.Local.Gateways.DataSources
{
    public interface ICombatCharacterDataBase
    {
        bool TryGet(CharacterKey key, out CharacterDefinitionModel value);
    }

    public interface ICharacterPrefabDataSource
    {
        GameObject Get(ModelName model);
    }
}
