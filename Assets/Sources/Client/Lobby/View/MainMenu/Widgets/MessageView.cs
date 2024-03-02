using Client.Lobby.Core.Chat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Lobby.View.MainMenu.Widgets
{
    public class MessageView : MonoBehaviour
    {
        [SerializeField] private Image _picture;
        [SerializeField] private TMP_Text _message;

        public void SetMessage(Message message)
        {
            _message.text = message.Text;
        }
    }
}
