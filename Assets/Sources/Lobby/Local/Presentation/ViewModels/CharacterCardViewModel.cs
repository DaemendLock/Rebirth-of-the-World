using Lobby.Common.Primitives;

using UnityEngine;

namespace Lobby.Local.Presentation.ViewModels
{
    public sealed class CharacterCardViewModel
    {
        public CharacterKey CharacterId;
        public string Name;
        public Sprite Icon;
        public bool IsAvailable;

        public CharacterCardViewModel(CharacterKey characterId)
        {
            CharacterId = characterId;
        }
    }
}
