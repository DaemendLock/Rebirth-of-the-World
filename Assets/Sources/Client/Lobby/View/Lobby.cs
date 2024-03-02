using Client.Lobby.Core.Characters;
using Client.Lobby.View.ScenarionSelection;
using Client.Lobby.View.TeamSetup.Widgets;
using Client.Lobby.View.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

namespace Client.Lobby.View
{
    public interface LoadingService
    {

    }

    public class Lobby
    {
        private readonly MainMenu.MainMenu _mainMenu;
        private readonly Gallery.Gallery _gallery;
        private readonly CharacterSheet.CharacterSheet _characterSheet;

        private readonly Stack<IMenuElement> _backList;

        public static Lobby Instance { get; private set; }

        public Lobby(MainMenu.MainMenu mainMenu, Gallery.Gallery gallery, CharacterSheet.CharacterSheet characterSheet)
        {
            if (Instance != null)
            {
                throw new InvalidOperationException();
            }

            Instance = this;

            _mainMenu = mainMenu;
            _gallery = gallery;
            _characterSheet = characterSheet;

            _backList = new Stack<IMenuElement>();

            _backList.Push(_mainMenu);
        }

        public void OpenMenu(IMenuElement menu)
        {
            _backList.Peek().SetActive(false);
            _backList.Push(menu);
            menu.SetActive(true);
        }

        public void GoBack()
        {
            if (_backList.Count == 1)
            {
                return;
            }

            _backList.Pop().SetActive(false);
            _backList.Peek().SetActive(true);
        }

        public void Home()
        {
            _backList.Peek()?.SetActive(false);
            _backList.Clear();
            _backList.Push(_mainMenu);
            _mainMenu.SetActive(true);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void ViewCharacter(Character character)
        {
            _characterSheet.SetCharacter(character);
            OpenMenu(_characterSheet);
        }

        public void OpenCharacterSelection(CharacterSlotWidget target)
        {
            OpenMenu(_gallery);
        }
    }
}