using Assets.Sources.Temp;
using Assets.Sources.Temp.Template;

using Client.Lobby.Core.Accounts;
using Client.Lobby.Core.Characters;
using Client.Lobby.Core.Chat;
using Client.Lobby.Infrastructure.Adapters;
using Client.Lobby.Infrastructure.Controllers;
using Client.Lobby.Infrastructure.Controllers.Accounts;
using Client.Lobby.Infrastructure.Factories;
using Client.Lobby.Infrastructure.Input;
using Client.Lobby.Infrastructure.Providers;
using Client.Lobby.Networking;
using Client.Lobby.View.CharacterSheet;
using Client.Lobby.View.Gallery;
using Client.Lobby.View.Gallery.Widgets;
using Client.Lobby.View.MainMenu;
using Client.Lobby.View.MainMenu.Widgets;

using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

using Utils.Patterns.Adapters;
using Utils.Patterns.Factory;

namespace Temp.Testing
{
    [RequireComponent(typeof(AssetProvider), typeof(DataIniter))]
    internal class LobbyIniter : MonoBehaviour
    {
        [SerializeField] private ChatWidget _chatWidget;
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private Gallery _gallery;
        [SerializeField] private CharacterSheet _characterSheet;

        private AssetProvider _assetProvider;
        private DataIniter _dataIniter;
        private LobbyClient _lobbyClient;
        private readonly LobbyInput _input;
        private InputStateMachine _inputStateMachine;

        private AccountsController _accountsController;
        private ServerCommandAdapter _serverCommandAdapter;
        private Client.Lobby.Infrastructure.Providers.DefaultCharactersProvider _charactersProvider;

        private FactoryCache _factories;
        private ControllersCache _controllers;

        private async void Start()
        {
            _assetProvider = GetComponent<AssetProvider>();
            _dataIniter = GetComponent<DataIniter>();

            Client.Lobby.View.Lobby lobby = new(_mainMenu, _gallery, _characterSheet);

            _inputStateMachine = new();

            //_inputStateMachine.ChangeState<MainMenuInputState>();

            _accountsController = new();

            await LoadLocalData();

            _factories = CreateFactories();
            _controllers = CallFactories();

            _lobbyClient = new LobbyClient(_serverCommandAdapter);
            _lobbyClient.Connect();
            RequestInitialData();
        }

        private async Task LoadLocalData()
        {
            Account account = _accountsController.GetAccount(AccountsDataProvider.ActiveAccount);

            Adapter<Character, Data.Characters.Character> adapter = new CharacterDataAdapter(account);

            DefaultCharactersProvider dataDefaultCharactersProvider = new(_dataIniter, adapter);
            await dataDefaultCharactersProvider.Load();
            _charactersProvider = dataDefaultCharactersProvider;
        }

        private FactoryCache CreateFactories()
        {
            Account account = _accountsController.GetAccount(AccountsDataProvider.ActiveAccount);

            Factory<CharacterCardWidget> characterCardFactory = new CharacterCardFactory(_assetProvider);
            Factory<MessageView, Message> messageFactory = new ChatMessageFactory(_assetProvider);
            Factory<ChatController, ChatWidget> chatFactory = new ChatControllerFactory(messageFactory);
            Factory<GalleryController, Gallery> galleryFactory = new GalleryControllerFactory(characterCardFactory, _charactersProvider.GetCharacters(), account);

            return new FactoryCache(chatFactory, galleryFactory);
        }

        private ControllersCache CallFactories()
        {
            ChatController chatController = _factories.ChatFactory.Create(_chatWidget);
            GalleryController galleryController = _factories.GalleryFactory.Create(_gallery);

            _serverCommandAdapter = new(_accountsController, chatController, galleryController);

            return new(chatController, galleryController);
        }

        private class GetInitialDataRequest : Request
        {
            public byte[] GetBytes() => new byte[0];
        }

        private void RequestInitialData() => _lobbyClient.SendRequest(new GetInitialDataRequest());

        private readonly struct FactoryCache
        {
            public readonly Factory<ChatController, ChatWidget> ChatFactory;
            public readonly Factory<GalleryController, Gallery> GalleryFactory;

            public FactoryCache(Factory<ChatController, ChatWidget> chatFactory, Factory<GalleryController, Gallery> galleryFactory)
            {
                ChatFactory = chatFactory;
                GalleryFactory = galleryFactory;
            }
        }

        private readonly struct ControllersCache
        {
            public readonly ChatController _chatController;
            public readonly GalleryController GalleryController;

            public ControllersCache(ChatController chatController, GalleryController galleryController)
            {
                _chatController = chatController;
                GalleryController = galleryController;
            }
        }
    }

    internal class DefaultCharactersProvider : Client.Lobby.Infrastructure.Providers.DefaultCharactersProvider
    {
        private readonly DataIniter _dataIniter;
        private List<Character> _loadedCharacters;
        private readonly Adapter<Character, Data.Characters.Character> _adapter;

        public DefaultCharactersProvider(DataIniter dataIniter, Adapter<Character, Data.Characters.Character> adapter)
        {
            _dataIniter = dataIniter;
            _adapter = adapter;
        }

        public List<Character> GetCharacters() => _loadedCharacters;

        public async Task Load() => _loadedCharacters = await _dataIniter.LoadCharacters(_adapter);
    }
}
