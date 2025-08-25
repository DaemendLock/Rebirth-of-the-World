using Client.Lobby.Infrastructure.Providers;
using Client.Lobby.View.Gallery.Widgets;

using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Factories
{
    public class CharacterCardFactory : Factory<CharacterCardWidget>
    {
        private readonly CharacterCardWidget _prefab;

        public CharacterCardFactory(AssetProvider assetProvider)
        {
            _prefab = assetProvider.CharacterCardWidgetPrefab;
        }

        public CharacterCardWidget Create()
        {
            CharacterCardWidget result = UnityEngine.Object.Instantiate(_prefab);
            return result;
        }
    }
}
