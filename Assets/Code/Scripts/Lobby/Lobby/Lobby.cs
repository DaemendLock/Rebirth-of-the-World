using Networking;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lobby : MonoBehaviour {

    [SerializeField] private GameObject lobby;
    [SerializeField] private GameObject sceneSelection;
    [SerializeField] private TeamSelection teamSelection;
    [SerializeField] private GameObject gallery;
    [SerializeField] private MemberPreview memberPreview;

    [Header("Text objects")]
    [SerializeField] private TMP_Text lvlText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text goldAmmountText;
    [SerializeField] private TMP_Text guildTokenAmmountText;

    [Header("Test Account")]
    [SerializeField] private Account _account;
    public static Account ActiveAccount ;
    

    private Stack<GameObject> backList = new Stack<GameObject>();

    public void Start() {
        ActiveAccount = _account;
        backList.Push(lobby);
       
        if(ServerManager.ActiveAccount != null)
            _account = ServerManager.ActiveAccount;
        nameText.text = _account.data.Nickname;
        lvlText.text = "Lv." + _account.data.Lvl.ToString();
        goldAmmountText.text = _account.data.Currencies.currencies[CurrencyType.GOLD].ToString();
        guildTokenAmmountText.text = _account.data.Currencies.currencies[CurrencyType.GUILD_TOKENS].ToString();
    }

    public void OpenMenu(GameObject menu) {
        backList.Peek().SetActive(false);
        backList.Push(menu);
        menu.SetActive(true);
    }

    public void BackMenu() {
        if (backList.Count == 1)
            return;
        backList.Pop().SetActive(false);
        backList.Peek().SetActive(true);
    }

    public void Home() {
        backList.Peek()?.SetActive(false);
        backList.Clear();
        backList.Push(lobby);
        lobby.SetActive(true);
    }

    public void StartScenario() {
        Scenario scenario = new FlorenceTrial();
        OpenMenu(teamSelection.gameObject);
        teamSelection.PrepareFor = scenario;
        teamSelection.SetupSelection();
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void ViewCharacter(GalleryCard card) {
        Preview(card.Unit);
    }

    public void Preview(UnitPreview unit) {
        memberPreview.InsertUnit(unit);
        OpenMenu(memberPreview.gameObject);
    }
}