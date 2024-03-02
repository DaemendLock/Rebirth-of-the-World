using Client.Lobby.Core.Chat;
using System.Collections.Generic;
using UnityEngine;
using Utils.DataStructure;

namespace Client.Lobby.View.MainMenu.Widgets
{
    public class ChatWidget : MonoBehaviour
    {
        [SerializeField] private int _maxMessageCount;

        private ObjectPool<MessageView> _objectPool;
        private List<MessageView> _messages;

        public void ShowChannel(Channel channel)
        {
            Clear();
        }

        public void Clear()
        {

        }
    }
}
