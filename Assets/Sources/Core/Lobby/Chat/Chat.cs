using System;
using System.Collections.Generic;

namespace Core.Lobby.Chat
{
    public static class Chat
    {
        public static event Action<Message> OnMessage;

        private static List<Channel> _channels = new();

        public static void PostMessage(int channelId, int authorId, string message)
        {

        }
    }
}