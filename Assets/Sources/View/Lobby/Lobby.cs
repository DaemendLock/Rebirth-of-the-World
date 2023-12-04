using Core.Lobby.Characters;
using Core.Lobby.Encounters;
using Data.Characters;
using System.Collections.Generic;
using UnityEngine;
using View.Lobby.CharacterSheet;
using View.Lobby.TeamSetup.Widgets;
using View.Lobby.Utils;

namespace View.Lobby
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private MainMenu.MainMenu _lobby;
        [SerializeField] private ScenarionSelection.ScenarioSelection _sceneSelection;
        [SerializeField] private TeamSetup.TeamSetup _teamSelection;
        [SerializeField] private Gallery.Gallery _gallery;
        [SerializeField] private CharacterSheet.CharacterSheet _characterSheet;

        private Stack<MenuElement> _backList = new Stack<MenuElement>();

        public static Lobby Instance { get; private set; }

        public ViewMode CharacterSheetMode
        {
            get => _characterSheet.ViewMode;
            set => _characterSheet.ViewMode = value;
        }

        public void Start()
        {
            if (Instance != null)
            {
                enabled = false;
                return;
            }

            Instance = this;

            _backList.Push(_lobby);

            _lobby.gameObject.SetActive(true);
            _sceneSelection.gameObject.SetActive(false);
            _teamSelection.gameObject.SetActive(false);
            _gallery.gameObject.SetActive(false);
            _characterSheet.gameObject.SetActive(false);
        }

        public void OpenMenu(MenuElement menu)
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
            _backList.Push(_lobby);
            _lobby.SetActive(true);
        }

        public void StartScenario(Encounter encounter)
        {
            _teamSelection.SetUp(encounter);
            //OpenMenu(_teamSelection);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void ViewCharacter(Character character, CharacterState data)
        {
            _characterSheet.InsertCharacter(character, data);
            OpenMenu(_characterSheet);
        }

        public void OpenCharacterSelection(CharacterSlotWidget target)
        {
            OpenMenu(_gallery);
            _characterSheet.ViewMode = ViewMode.Select;
            _characterSheet.SelectionTarget = target;
        }
    }
}