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

        //ServerManager.RegisterAccount(login.text, password.text);
    }

    private void OnEnable() {
        
    }

    private void OnDisable() {
        
    }

    //private void OnRegisterRequestResolved(AccountAccessResponse access) {
    //    if (access.Success) {
    //        failMsg.text = "";
    //        gameObject.SetActive(false);
    //    } else {
    //        failMsg.text = "Wrong login or password";
    //    }
    //}
}
