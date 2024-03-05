using System.Text;

using Client.Lobby.Core.Chat;
using Client.Lobby.Infrastructure.Controllers;
using Client.Lobby.Infrastructure.Controllers.Accounts;

using UnityEngine;

using Utils.ByteHelper;
using Utils.DataTypes;
using Utils.Patterns.Adapters;
using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Adapters
{
    public interface ServerCommand
    {
        void Perform();
    }

    public class SetCharacterData : ServerCommand
    {
        private AccountsController _accountsController;
        private int _accountId;
        private int _characterId;
        private int _activeViewSet;
        private ProgressValue _level;
        private ProgressValue _affection;
        private SpellId[] _spells;
        private ItemId[] _gear;

        public void Perform()
        {
            // _accountsController.UpdateCharactersData();
        }
    }

    public class ServerCommandAdapter : Adapter<ServerCommand, byte[]>
    {
        private readonly Factory<Message, MessageData> _messageDataParser;

        private readonly AccountsController _accountsController;
        private readonly ChatController _chatController;
        private readonly GalleryController _galleryController;

        public ServerCommandAdapter(AccountsController accountsController, ChatController chatController, GalleryController galleryController)
        {
            _chatController = chatController;
            _accountsController = accountsController;
            _galleryController = galleryController;
        }

        public ServerCommand Adapt(byte[] value)
        {
            ByteReader source = new(value);
            return new PrintMessageCommand(source.ReadString(Encoding.Unicode));
        }

        //private ServerCommand PasreCharacterData(ByteReader source)
        //{

        //}

        private class PrintMessageCommand : ServerCommand
        {
            private string _message;

            public PrintMessageCommand(string message)
            {
                _message = message;
            }

            public void Perform() => Debug.Log(_message);
        }
    }
}
