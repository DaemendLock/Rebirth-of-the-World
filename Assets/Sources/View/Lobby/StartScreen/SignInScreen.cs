using UnityEngine;

public class SignInScreen : MonoBehaviour
{

    [SerializeField] private GameObject loadingScene;
    [SerializeField] private GameObject authWindow;
    [SerializeField] private GameObject registerWindow;
    [SerializeField] private GameObject failedConnectionWindow;
    [SerializeField] private GameObject startWindow;

    //private int connectAttempts = 0;

    //private Account _account;

    private void Start()
    {
        //connectAttempts = 0;
        //if (ServerManager.TryConnect())
        //{
        //    RequestAuth();
        //}
        //else
        //{
        //    Reconect();
        //}
    }

    private void OnEnable()
    {
        //ServerManager.AccountAccessFigured += OnAccountAccessed;
    }

    private void OnDisable()
    {
        //ServerManager.AccountAccessFigured -= OnAccountAccessed;
    }

    public void Reconect()
    {
        //failedConnectionWindow.SetActive(false);
        //if (!ServerManager.TryConnect())
        //{
        //    failedConnectionWindow.SetActive(true);
        //}
        //RequestAuth();
    }

    //private void OnAccountAccessed(AccountAccessResponse response)
    //{
    //    if (!response.Success)
    //        return;

    //    startWindow.SetActive(true);
    //    Debug.Log("Loginned");
    //    _account = new(response.UID);

    //}

    public void SwitchSigninAndLogin()
    {
        if (authWindow.activeSelf)
        {
            authWindow.SetActive(false);
            registerWindow.SetActive(true);
            return;
        }
        registerWindow.SetActive(false);
        authWindow.SetActive(true);
    }

    public void StartGame()
    {
        //ServerManager.RequestAccountData(_account.UID, AccountsData.DataType.ACCOUNT_CUSTOMIZATION, 0);
        //while (ServerManager.ActiveAccount?.Data.Nickname.Length == 0)
        //{

        //}
        Loader.Instance.LoadScene(1);
    }

    public void Logout()
    {
        //_account = null;
        //ServerManager.InfoLogout();
        startWindow.SetActive(false);
        RequestAuth();
    }

    private void RequestAuth()
    {
        authWindow.SetActive(true);
    }
}
