using System.Runtime.CompilerServices;

using Client.Lobby.Infrastructure.Controllers;
using Client.Lobby.Infrastructure.Networking;
using Client.Lobby.Infrastructure.Networking.Requests;

using Client.Lobby.Domain.Accounts;
using Client.Lobby.Domain.Characters;

using Utils.ByteHelper;
using Utils.Patterns.Adapters;
using Utils.DataTypes;
using Utils.Patterns.Factory;

namespace Client.Lobby.Infrastructure.Adapters
{
    public class ServerCommandAdapter : Adapter<ServerCommand, byte[]>
    {
        private readonly AccountsController _accountsController;
        private readonly ChatController _chatController;
        private readonly GalleryController _galleryController;
        private readonly ActiveAccountController _activeAccountController;

        private readonly Factory<Character, CharacterData> _characterFactory;

        public ServerCommandAdapter(AccountsController accountsController, ChatController chatController, GalleryController galleryController, ActiveAccountController activeAccountController, Factory<Character, CharacterData> characterFactory)
        {
            _chatController = chatController;
            _accountsController = accountsController;
            _galleryController = galleryController;
            _activeAccountController = activeAccountController;
            _characterFactory = characterFactory;
        }

        public ServerCommand Adapt(byte[] value)
        {
            ByteReader source = new(value);

            return (ServerCommandType) source.ReadByte() switch
            {
                ServerCommandType.PrintDebug => new PrintMessageCommand(source.ReadString()),
                ServerCommandType.SetAccountInfo => HandleAccountCommand(source),
                _ => null,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ServerCommand HandleAccountCommand(ByteReader source)
        {
            AccountDataType type = (AccountDataType) source.ReadByte();

            int accountId = source.ReadInt();

            Account account = new(accountId, null, null);

            if (type.HasFlag(AccountDataType.Name))
            {
                account.SetName(source.ReadString());
            }

            if (type.HasFlag(AccountDataType.Level))
            {
                account.SetLevel(ProgressValue.Parse(source));
            }

            if (type.HasFlag(AccountDataType.CharacterData))
            {
                ParseCharactersData(account, source);
            }

            return new SetAccountData(account, type, _accountsController);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ParseCharactersData(Account account, ByteReader source)
        {
            CharacterDataParams @params = (CharacterDataParams) source.ReadByte();

            bool fullData = @params.HasFlag(CharacterDataParams.FullData);

            ushort count;

            if (@params.HasFlag(CharacterDataParams.AllCharcters))
            {
                count = source.ReadUShort();
            }
            else
            {
                count = 1;
            }

            for (ushort i = 0; i < count; i++)
            {
                Character character = ParseNextCharacter(account.Id, fullData, source);
                account.Characters.ViewCharacter(character);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Character ParseNextCharacter(int accountId, bool full, ByteReader source)
        {
            CharacterData characterData;

            if (full)
            {
                characterData = new CharacterData(accountId, source.ReadInt(), source.ReadByte(), source.ReadByte(), ProgressValue.Parse(source), ProgressValue.Parse(source), new ItemId[8]);
            }
            else
            {
                characterData = new CharacterData(accountId, source.ReadInt(), source.ReadByte());
            }

            return _characterFactory.Create(characterData);

        }
    }
}
