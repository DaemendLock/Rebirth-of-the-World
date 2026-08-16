using Lobby.Common.Primitives;

namespace Lobby.Local.Presentation.ViewModels
{
    public sealed class CharacterCardViewModel
    {
        public CharacterKey CharacterId;
        public bool IsAvailable;

        public CharacterCardViewModel(CharacterKey characterId)
        {
            CharacterId = characterId;
        }
    }
}
