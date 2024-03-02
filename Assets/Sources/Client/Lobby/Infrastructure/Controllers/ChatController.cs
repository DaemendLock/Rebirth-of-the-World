using System;

using Client.Lobby.Core.Chat;
using Client.Lobby.View.MainMenu.Widgets;

using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Controllers
{
    public interface MessageData
    {
        public int SenderId { get; }
        public string Message { get; }
    }

    public class ChatController
    {
        private readonly Chat _model;
        private readonly ChatWidget _view;
        private readonly Factory<MessageView, Message> _viewFactory;

        public ChatController(Chat model, ChatWidget view, Factory<MessageView, Message> viewFactory)
        {
            _model = model;
            _view = view;
            _viewFactory = viewFactory;
        }

        public void PostChatMessage(int channelId, Message message)
        {
            Channel postChannel = _model.FindChannel(channelId) ?? AddChannel(channelId);
            postChannel.PostMessage(message);
        }

        public Channel AddChannel(int channelId)
        {
            throw new NotImplementedException();
            //_view.CreateChannel();
        }
    }
}
