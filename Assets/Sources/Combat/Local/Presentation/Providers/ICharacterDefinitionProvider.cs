using Combat.Common.Primitives;

using UnityEngine;

namespace Combat.Local.Presentation.Providers
{
     public interface ICharacterPrefabProvider
    {
        GameObject Get(ModelName model);
    }
}
