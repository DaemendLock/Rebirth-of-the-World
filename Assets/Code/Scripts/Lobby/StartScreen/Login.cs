using Networking;
using System.Collections;
using TMPro;
using UnityEngine;

public class Login : MonoBehaviour {
    [SerializeField] private TMP_InputField _login;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private TMP_Text _failMsg;

    public void TryLogin() {
        ResponsableRequest request = ServerManager.LoginAttempt(_login.text, _password.text);
        StartCoroutine(WaitForAccess(request));
    }

    private IEnumerator WaitForAccess(ResponsableRequest request) {
        while (request.Response == null) {
            yield return null;
        }
        OnAccountAccessResolved(new AccountAccessResponse(request.Response));
    }

    private void OnAccountAccessResolved(AccountAccessResponse access) {
        if (access.Success) {
            _failMsg.text = "";
            gameObject.SetActive(false);
        } else {
            _failMsg.text = "Wrong login or password";
        }
    }
}
