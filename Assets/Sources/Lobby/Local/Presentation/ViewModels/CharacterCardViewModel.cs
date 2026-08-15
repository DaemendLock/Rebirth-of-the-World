using Lobby.Common.Primitives;

namespace Lobby.Local.Presentation.ViewModels
{
    public sealed class CharacterCardViewModel
    {
        public CharacterId CharacterId;
        public bool IsAvailable;

        public CharacterCardViewModel(CharacterId characterId)
        {
            CharacterId = characterId;
        }
    }
}
