using Client.Lobby.Core.Chat;
using Client.Lobby.Infrastructure.Controllers;
using Client.Lobby.Infrastructure.Providers;
using Client.Lobby.View.MainMenu.Widgets;
using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Factories
{
    public class ChatControllerFactory : Factory<ChatController, ChatWidget>
    {
        private Chat _chat;
        private Factory<MessageView, Message> _viewFactory;

        public ChatControllerFactory(Factory<MessageView, Message> viewFactory)
        {
            _chat = new Chat();
            _viewFactory = viewFactory;
        }

        public ChatController Create(ChatWidget view)
        {
            _chat.AddChannel(0);

            return new(_chat, view, _viewFactory);
        }
    }

    public class ChatMessageFactory : Factory<MessageView, Message>
    {
        private MessageView _prefab;

        public ChatMessageFactory(AssetProvider assetProvider)
        {
            _prefab = assetProvider.MessageViewPrefab;
        }

        public MessageView Create(Message data)
        {
            MessageView result = UnityEngine.Object.Instantiate(_prefab);
            result.SetMessage(data);
            return result;
        }
    }
}
