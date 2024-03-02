using Client.Lobby.View.MainMenu.Widgets;
using UnityEngine;
using Client.Lobby.View.Utils;

namespace Client.Lobby.View.MainMenu
{
    public class MainMenu : MenuElement
    {
        [SerializeField] private ProfileWidget _profileWidget;
        [SerializeField] private ResourceBoardWidget _resourceWidget;
        [SerializeField] private ChatWidget _chatWidget;
    }
}
