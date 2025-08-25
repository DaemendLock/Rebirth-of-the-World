using System;
using System.Collections.Generic;

using UnityEngine;

using Client.Lobby.Domain.Characters;
using Client.Lobby.View.Utils;

namespace Client.Lobby.View
{
    public class Lobby : MonoBehaviour
    {
        private readonly Stack<IMenuElement> _backList = new ();

        private void Start()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException();
            }

            Instance = this;

            _backList.Push(MainMenu);
        }

        public static Lobby Instance { get; private set; }

        [field: SerializeField] public MainMenu.MainMenu MainMenu { get; private set; }
        [field: SerializeField] public Gallery.Gallery Gallery { get; private set; }
        [field: SerializeField] public CharacterSheet.CharacterSheet CharacterSheet { get; private set; }

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
            _backList.Push(MainMenu);
            MainMenu.SetActive(true);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void ViewCharacter(Character character)
        {
            CharacterSheet.Bind(character);
            OpenMenu(CharacterSheet);
        }
    }
}