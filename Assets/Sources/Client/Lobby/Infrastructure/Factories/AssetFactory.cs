using Client.Lobby.Infrastructure.Providers;
using Client.Lobby.Core.Chat;
using Client.Lobby.View.Gallery.Widgets;
using Client.Lobby.View.MainMenu.Widgets;
using Client.Lobby.View.Gallery;

using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Factories
{
    public class AssetFactory : Factory<CharacterCardWidget, Gallery>, Factory<MessageView, Message>
    {
        private readonly AssetProvider _assetProvider;

        public AssetFactory(AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public CharacterCardWidget Create(Gallery target)
        {
            CharacterCardWidget result = UnityEngine.Object.Instantiate(_assetProvider.CharacterCardWidgetPrefab);
            target.AddCard(result);
            return result;
        }

        public MessageView Create(Message data)
        {
            MessageView result = UnityEngine.Object.Instantiate(_assetProvider.MessageViewPrefab);
            result.SetMessage(data);
            return result;
        }
    }
}
