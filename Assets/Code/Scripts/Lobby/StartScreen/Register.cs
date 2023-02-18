using Networking;
using System.Collections;
using TMPro;
using UnityEngine;

public class Register : MonoBehaviour {

    [SerializeField] private TMP_InputField login;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField confirm_password;
    [SerializeField] private TMP_Text failMsg;

    public void TryRegister() {
        if (confirm_password.text != password.text) {
            failMsg.text = confirm_password.text;
            return;
        }
        ResponsableRequest request = ServerManager.RegisterAccount(login.text, password.text);
        StartCoroutine(WaitForAccess(request));
    }

    private IEnumerator WaitForAccess(ResponsableRequest request) {
        while (request.Response == null) {
            yield return null;
        }
        OnRegisterRequestResolved(new AccountAccessResponse(request.Response));
    }

    private void OnRegisterRequestResolved(AccountAccessResponse access) {
        if (access.Success) {
            failMsg.text = "";
            gameObject.SetActive(false);
        } else {
            failMsg.text = "Wrong login or password";
        }
    }
}
