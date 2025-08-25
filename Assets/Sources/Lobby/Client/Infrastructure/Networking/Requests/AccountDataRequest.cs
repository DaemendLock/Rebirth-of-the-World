using System;
using System.Buffers.Binary;

using Client.Lobby.Domain.Accounts;

using Utils.Networking;

namespace Client.Lobby.Infrastructure.Networking.Requests
{
    [Flags]
    public enum CharacterDataParams : byte
    {
        ShortData = 0,
        FullData = 1,
        AllCharcters = 2,
    }

    public class AccountDataRequest : Request
    {
        private readonly int _id;
        private readonly AccountDataType _type;
        private readonly int[] _args;

        public AccountDataRequest(int id, AccountDataType type, params int[] args)
        {
            _id = id;
            _type = type;
            _args = args;
        }

        public byte[] GetBytes()
        {
            byte[] bytes;

            if (_type.HasFlag(AccountDataType.CharacterData))
            {
                CharacterDataParams @params = (CharacterDataParams) _args[0];

                if (@params.HasFlag(CharacterDataParams.AllCharcters))
                {
                    bytes = new byte[7];
                }
                else
                {
                    bytes = new byte[11];
                    BinaryPrimitives.WriteInt32BigEndian(new Span<byte>(bytes, 7, sizeof(int)), _args[1]);
                }

                bytes[6] = (byte) @params;
            }
            else
            {
                bytes = new byte[6];
            }

            bytes[0] = (byte) RequestType.GetAccountData;
            BinaryPrimitives.WriteInt32BigEndian(new Span<byte>(bytes, 1, sizeof(int)), _id);
            bytes[5] = (byte) _type;
            return bytes;
        }
    }
}
