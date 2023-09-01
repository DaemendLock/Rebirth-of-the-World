using Networking;
using TMPro;
using UnityEngine;

public class Login : MonoBehaviour {
    [SerializeField] private TMP_InputField _login;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private TMP_Text _failMsg;

    public void TryLogin() {
        //ServerManager.LoginAttempt(_login.text, _password.text);
    }

    private void OnEnable() {
        //ServerManager.AccountAccessFigured += OnAccountAccessResolved;
    }

    private void OnDisable() {
        //ServerManager.AccountAccessFigured -= OnAccountAccessResolved;
    }

    private void OnAccountAccessResolved() {
        
    }
}
