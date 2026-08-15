using Lobby.Common.Primitives;

using UnityEngine;

namespace Lobby.Local.Presentation.Misc
{
    public interface IAssetProvider
    {
        Sprite GetCharacterIcon(CharacterId id);
    }
}
