using Core.Lobby.Chat;
using System.Collections.Generic;
using UnityEngine;

namespace View.Lobby.MainMenu.Widgets
{
    internal class ChatWidget : MonoBehaviour
    {
        [SerializeField] private int _maxMessageCount;
        [SerializeField] private ChatMessage _prefab;

        private List<ChatMessage> _messages;

        private void OnEnable()
        {
            //Chat.OnMessage += 
        }

        private void OnDisable() { }

        public void ShowChannel(Channel channel)
        {

        }

        public void Clear()
        {

        }
    }
}
