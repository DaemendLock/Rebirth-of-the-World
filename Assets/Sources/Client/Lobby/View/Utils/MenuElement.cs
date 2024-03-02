using UnityEngine;

namespace Client.Lobby.View.Utils
{
    public class MenuElement : MonoBehaviour, IMenuElement
    {
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }

    public interface IMenuElement
    {
        public void SetActive(bool active);
    }
}
