using Lobby.Common.Primitives;
using Lobby.Local.Presentation.Misc;

using UnityEngine;

namespace Assets.Sources.Testing.Local.Lobby.Temp
{
    public sealed class LazyAssetProvider : MonoBehaviour, IAssetProvider
    {
        [SerializeField] private LocalCharacterDatabase _characterDatabase;

        public Sprite GetCharacterIcon(CharacterId id)
        {
            foreach (CharacteData val in _characterDatabase.CharactersData)
            {
                if (val.Id != id.Value)
                {
                    continue;
                }

                return val.Icon;
            }

            return null;
        }
    }
}
