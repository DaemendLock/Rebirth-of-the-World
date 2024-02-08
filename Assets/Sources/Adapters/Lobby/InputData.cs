using Data.Characters;
using System;
using System.IO;
using System.Text;
using Utils.DataTypes;

namespace Adapters.Lobby
{
    public readonly struct ShowChatMessageCommand : InputData
    {
        private const int MESSAGE_LENGHT_BYTE_INDEX = 8;

        public readonly int ChannelId;
        public readonly int AuthorId;
        public readonly string Message;

        public ShowChatMessageCommand(int channelId, int authorId, string message)
        {
            ChannelId = channelId;
            AuthorId = authorId;
            Message = message;
        }

        public byte[] GetBytes()
        {
            byte[] result;

            using (MemoryStream target = new(sizeof(int) * 2 + 1 + Message.Length * sizeof(char)))
            {
                target.WriteByte((byte) ServerCommand.ShowChatMessage);
                target.Write(BitConverter.GetBytes(ChannelId));
                target.Write(BitConverter.GetBytes(AuthorId));
                target.Write(BitConverter.GetBytes(Message.Length));
                target.Write(Encoding.Unicode.GetBytes(Message));

                result = target.ToArray();
            }

            return result;
        }

        public static ShowChatMessageCommand Parse(byte[] data, int start)
        {
            int channel = BitConverter.ToInt32(data, start);
            int author = BitConverter.ToInt32(data, start);
            string message = Encoding.Unicode.GetString(data, start + sizeof(int) * 2, BitConverter.ToInt32(data, start + sizeof(int) * 2));

            return new(channel, author, message);
        }
    }

    public readonly struct UpdateCharacterDataCommand : InputData
    {
        public readonly int CharacterId;
        public readonly int AccountId;
        public readonly CharacterState Data;

        public UpdateCharacterDataCommand(int characterId, int accountId, CharacterState data)
        {
            CharacterId = characterId;
            AccountId = accountId;
            Data = data;
        }

        public byte[] GetBytes()
        {
            byte[] result;

            using (MemoryStream target = new(sizeof(int)))
            {
                target.WriteByte((byte) ServerCommand.ShowChatMessage);
                target.Write(BitConverter.GetBytes(CharacterId));
                target.Write(BitConverter.GetBytes(AccountId));

                //target.Write <CharacterData>

                result = target.ToArray();
            }

            return result;
        }

        public static UpdateCharacterDataCommand Parse(byte[] data, int start)
        {
            int id = BitConverter.ToInt32(data, start);

            return new();
        }
    }

    public readonly struct StartCombatCommand
    {
        // controlGroup
        private readonly UnitCreationData[] _units;

        public StartCombatCommand(UnitCreationData[] units)
        {
            _units = units;
        }

        internal static StartCombatCommand Parse(byte[] data, int start)
        {
            UnitCreationData[] units = new UnitCreationData[data[start]];

            return new StartCombatCommand();
        }
    }
}
