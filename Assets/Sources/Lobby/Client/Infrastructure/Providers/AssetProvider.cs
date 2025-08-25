using Client.Lobby.View.Gallery.Widgets;
using Client.Lobby.View.MainMenu.Widgets;
using UnityEngine;

namespace Client.Lobby.Infrastructure.Providers
{
    public class AssetProvider : MonoBehaviour
    {
        [SerializeField] private MessageView _messageViewPrefab;
        [SerializeField] private CharacterCardWidget _characterCardWidgetPrefab;

        public MessageView MessageViewPrefab => _messageViewPrefab;

        public CharacterCardWidget CharacterCardWidgetPrefab => _characterCardWidgetPrefab;
    }
}
