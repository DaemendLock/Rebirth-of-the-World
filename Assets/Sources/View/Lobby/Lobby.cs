using System.Collections.Generic;
using UnityEngine;
using View.Lobby.CharacterSheet;
using View.Lobby.Gallery;
using View.Lobby.Utils;

namespace View.Lobby
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private MainMenu.MainMenu _lobby;
        [SerializeField] private GameObject _sceneSelection;
        [SerializeField] private TeamSelection _teamSelection;
        [SerializeField] private Gallery.Gallery _gallery;
        [SerializeField] private MemberPreview _memberPreview;

        [Header("Test Account")]
        //[SerializeField] private Account _account;
        //public static Account ActiveAccount;

        private Stack<MenuElement> _backList = new Stack<MenuElement>();

        public void Start()
        {
            /*
            ActiveAccount = _account;
            backList.Push(_lobby);

            if (ServerManager.ActiveAccount != null)
                _account = ServerManager.ActiveAccount;
            nameText.text = _account.Data.Nickname;
            lvlText.text = "Lv." + _account.Data.Lvl.ToString();
            goldAmmountText.text = _account.Data.Currencies.currencies[CurrencyType.GOLD].ToString();
            guildTokenAmmountText.text = _account.Data.Currencies.currencies[CurrencyType.GUILD_TOKENS].ToString();
            */
        }

        public void OpenMenu(MenuElement menu)
        {
            _backList.Peek().SetActive(false);
            _backList.Push(menu);
            menu.SetActive(true);
        }

        public void BackMenu()
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
            _teamSelection.SetupSelection();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void ViewCharacter(GalleryCard card)
        {
            //Preview(card.Unit);
        }

        public void Preview(UnitPreview unit)
        {
            _memberPreview.InsertUnit(unit);
            OpenMenu(_memberPreview);
        }
    }
}