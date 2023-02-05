using UnityEngine;

public sealed class PauseMenu : MonoBehaviour {
    public void ReturnToLobby() {
        Loader.Instance.LoadScene(1);
    } 
    public void ReturnToGame() {
        gameObject.SetActive(false);
    }
}
