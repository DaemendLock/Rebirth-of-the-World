using Client.Lobby.Core.Characters;
using Client.Lobby.Infrastructure.Input;
using Client.Lobby.View.Utils;

namespace Client.Lobby.Infrastructure.Controllers
{
    public interface CharactersDataProvider
    {
        
    }

    public class LobbyWindowController
    {
        private readonly LobbyInput _inputActions;
        private readonly CharactersDataProvider _characters;
        private readonly IMenuElement _mainMenu;

        private readonly View.Lobby _lobby;
        
        public void ViewCharacter(Character character)
        {
            
        }
    }
}
