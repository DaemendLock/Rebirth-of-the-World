using Core.Lobby.Chat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.Lobby.MainMenu.Widgets
{
    internal class ChatMessage : MonoBehaviour
    {
        [SerializeField] private Image _picture;
        [SerializeField] private TMP_Text _message;

        public void SetMessage(Message message)
        {

        }
    }
}
