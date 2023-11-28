using System.Collections.Generic;
using UnityEngine;
using View.Lobby.General.Charaters;
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

        private void OnEnable()
        {
            CharactersList.CharacterRegistered += _gallery.CreateCharacterFile;
        }

        private void OnDisable()
        {
            CharactersList.CharacterRegistered -= _gallery.CreateCharacterFile;
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

        public void StartScenario()
        {
            //Scenario scenario = new FlorenceTrial();
            OpenMenu(_teamSelection);
            //_teamSelection.PrepareFor = scenario;
            //_teamSelection.SetupSelection();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void ViewCharacter(Character data)
        {
            _characterSheet.InsertCharacter(data);
            OpenMenu(_characterSheet);
        }
    }
}