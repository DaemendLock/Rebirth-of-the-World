using Lobby.Local.Presentation.View;

using System.Collections.Generic;

using UnityEngine;

namespace Lobby.Local.Presentation.Misc
{
    public enum TabType
    {
        None = 0,
        MainMenu,
        ScenarioSelection,
        TeamSetup,
        CharacterGallery,
        CharacterSheet
    }

    public sealed class UiNavigationService : MonoBehaviour
    {
        private readonly Stack<LobbyTabWidget> _tabHistory = new();

        [SerializeField] private LobbyTabWidget _homeTab;
        private LobbyTabWidget _currentTab;

        private void Start()
        {
            GoHome();
        }

        public void OpenTab(LobbyTabWidget tab)
        {
            if (_currentTab == tab)
            {
                return;
            }

            _tabHistory.Push(_currentTab);
            ShowTab(tab);
        }

        public void GoBack()
        {
            if (_tabHistory.TryPop(out LobbyTabWidget previousTab) == false)
            {
                Debug.Log("Tab history is empty");
                return;
            }

            ShowTab(previousTab);
        }

        public void GoHome()
        {
            ShowTab(_homeTab);
            _tabHistory.Clear();
            _tabHistory.Push(_currentTab);
        }

        private void ShowTab(LobbyTabWidget tab)
        {
            if (_currentTab != null)
            {
                _currentTab.IsActive = false;
            }

            _currentTab = tab;

            if (tab == null)
            {
                return;
            }

            tab.IsActive = true;
        }
    }
}
