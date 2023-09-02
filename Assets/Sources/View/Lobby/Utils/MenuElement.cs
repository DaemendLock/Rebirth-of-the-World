using UnityEngine;

namespace View.Lobby.Utils
{
    public abstract class MenuElement : MonoBehaviour
    {
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
