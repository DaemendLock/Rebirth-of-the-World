using Client.Lobby.Core.Accounts;

using System.Collections.Generic;

namespace Client.Lobby.Core.Chat
{
    public class Chat
    {
        private readonly List<Channel> _channels = new();

        public Channel FindChannel(int channelId)
        {
            return _channels.Find((channel) => channel.Id == channelId);
        }

        public Channel AddChannel(int channelId)
        {
            Channel channel = new(channelId);
            _channels.Add(channel);
            return channel;
        }
    }

    public class Channel
    {
        public int Id;

        public Channel(int id)
        {
            Id = id;
            Messages = new();
        }

        public List<Message> Messages { get; }

        public void PostMessage(Message message)
        {
            Messages.Add(message);
        }
    }

    public class Message
    {
        public Account Sender;
        public string Text;

        public Message(string text, Account sender)
        {
            Sender = sender;
            Text = text;
        }
    }
}