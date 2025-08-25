using Client.Lobby.View.MainMenu.Widgets;
using UnityEngine;
using Client.Lobby.View.Utils;

namespace Client.Lobby.View.MainMenu
{
    public class MainMenu : MenuElement
    {
        [field: SerializeField] public ProfileWidget ProfileWidget { get; private set;}
        [SerializeField] private ResourceBoardWidget _resourceWidget;
        [field: SerializeField] public ChatWidget ChatWidget { get; private set; }

    }
}
