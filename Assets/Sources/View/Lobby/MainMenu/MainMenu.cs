using UnityEngine;
using View.Lobby.MainMenu.Widgets;
using View.Lobby.Utils;

namespace View.Lobby.MainMenu
{
    public class MainMenu : MenuElement
    {
        [SerializeField] private ProfileWidget _profileWidget;
        [SerializeField] private ResourceBoardWidget _resourceWidget;
        [SerializeField] private ChatWidget _chatWidget;
    }
}
