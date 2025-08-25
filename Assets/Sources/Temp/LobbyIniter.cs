using System.Collections;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using UnityEngine;

using Assets.Sources.Temp;

using Client.Lobby.Infrastructure.Adapters;
using Client.Lobby.Infrastructure.Controllers;
using Client.Lobby.Infrastructure.Factories;
using Client.Lobby.Infrastructure.Providers;
using Client.Lobby.Infrastructure.Networking;
using Client.Lobby.Infrastructure.Networking.Requests;
using Client.Lobby.Infrastructure.Networking.Protocol;

using Client.Lobby.View.Gallery;
using Client.Lobby.View.MainMenu.Widgets;

using Client.Lobby.Domain.Accounts;

using UtilsUnity.Networking;
using Utils.Patterns.Factory;
using Utils.Patterns.Adapters;
using Utils.ByteHelper;

namespace Temp.Testing
{
    internal class LobbyIniter : MonoBehaviour
    {
        [SerializeField] private Client.Lobby.View.Lobby _lobby;
        [SerializeField] private int _activeAccount;

        [SerializeField] private AssetProvider _assetProvider;
        [SerializeField] private DataIniter _dataIniter;

        [SerializeField] private string _ip;
        [SerializeField] private int _port;

        private SocketClient _client;
        private Protocol _protocol;

        private async void Start()
        {
            var val = await _dataIniter.LoadCharacters();
            CharactersProvider defaultCharacters = new DefaultCharactersProvider(val, new DefaultCharacterDataAdapter());

            ControllersCache cache = CallFactories(CreateFactories(defaultCharacters));

            ConnectToServer(cache);

            RequestInitialData(cache);
        }

        private void OnDestroy()
        {
            _client.Disconnect();
        }

        private FactoryCache CreateFactories(CharactersProvider defaultCharacters)
        {
            ChatMessageFactory chatMessageFactory = new(_assetProvider);
            CharacterCardFactory characterCardFActory = new(_assetProvider);
            ChatControllerFactory chatFactory = new(chatMessageFactory);
            GalleryControllerFactory galleryFactory = new(characterCardFActory, defaultCharacters);
            AccountsControllerFactory accountController = new();
            AccountsFactoryFactory accountFactoryFactory = new();

            return new(chatFactory, galleryFactory, accountController, accountFactoryFactory);
        }

        private ControllersCache CallFactories(FactoryCache factories)
        {
            _client = new SocketClient(new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp), new IPEndPoint(IPAddress.Parse(_ip), _port));

            ChatController chatController = factories.ChatFactory.Create(_lobby.MainMenu.ChatWidget);
            GalleryController galleryController = factories.GalleryFactory.Create(_lobby.Gallery);
            ActiveAccountController activeAccoutnController = new(_lobby.MainMenu.ProfileWidget, galleryController, _activeAccount);
            AccountsController accountsController = factories.AccountControllerFactory.Create(factories.AccountsFactoryFactory.Create(_client));

            return new(chatController, galleryController, activeAccoutnController, accountsController);
        }

        private void ConnectToServer(ControllersCache cache)
        {
            Adapter<ServerCommand, byte[]> serverCommandAdapter = new ServerCommandAdapter(cache.AccountsController, cache.ChatController, cache.GalleryController, cache.ActiveAccountController, new CharacterFactory(_client));

            _protocol = new LobbyProtocol(serverCommandAdapter);

            _client.Connect();
            StartCoroutine(ClientWork());
        }

        private IEnumerator ClientWork()
        {
            while (enabled)
            {
                HandleAll();
                yield return null;
            }
        }

        private void HandleAll()
        {
            while (_client.HasMessages)
            {
                ServerMessage message = _client.NextMessage();
                _protocol.Handle(message).Perform();
            }
        }

        private async void RequestInitialData(ControllersCache cache)
        {
            _client.SendRequest(new AccountDataRequest(_activeAccount, AccountDataType.Level | AccountDataType.Name | AccountDataType.CharacterData, (int) (CharacterDataParams.AllCharcters | CharacterDataParams.ShortData)));

            Account account = cache.AccountsController.GetAccount(_activeAccount);
            await Task.Delay(1000);
            cache.ActiveAccountController.SetActiveAccount(account);
        }

        private readonly struct FactoryCache
        {
            public readonly Factory<ChatController, ChatWidget> ChatFactory;
            public readonly Factory<GalleryController, Gallery> GalleryFactory;
            public readonly Factory<AccountsController, Factory<Account, int>> AccountControllerFactory;
            public readonly Factory<Factory<Account, int>, SocketClient> AccountsFactoryFactory;

            public FactoryCache(Factory<ChatController, ChatWidget> chatFactory, Factory<GalleryController, Gallery> galleryFactory, Factory<AccountsController, Factory<Account, int>> accountControllerFactory, Factory<Factory<Account, int>, SocketClient> accountsFactoryFactory)
            {
                ChatFactory = chatFactory;
                GalleryFactory = galleryFactory;
                AccountControllerFactory = accountControllerFactory;
                AccountsFactoryFactory = accountsFactoryFactory;
            }
        }

        private readonly struct ControllersCache
        {
            public readonly ChatController ChatController;
            public readonly GalleryController GalleryController;
            public readonly ActiveAccountController ActiveAccountController;
            public readonly AccountsController AccountsController;

            public ControllersCache(ChatController chatController, GalleryController galleryController, ActiveAccountController activeAccountController, AccountsController accountsController)
            {
                ChatController = chatController;
                GalleryController = galleryController;
                ActiveAccountController = activeAccountController;
                AccountsController = accountsController;
            }
        }
    }
}
