using Lobby.Common.Primitives;

using UnityEngine;

namespace Lobby.Local.Presentation.Misc
{
    public interface IAssetProvider
    {
        Sprite GetCharacterIcon(CharacterKey id);
        string GetCharacterName(CharacterKey id);
        Sprite GetCharacterPhoto(CharacterKey id);
    }
}
