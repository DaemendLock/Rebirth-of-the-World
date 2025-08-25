using Utils.ByteHelper;
using Utils.Patterns.Adapters;

namespace Client.Lobby.Infrastructure.Networking.Protocol
{
    public interface Protocol
    {
        ServerCommand Handle(ServerMessage message);
    }

    public class LobbyProtocol : Protocol
    {
        private readonly Adapter<ServerCommand, byte[]> _serverCommandAdapter;

        public LobbyProtocol(Adapter<ServerCommand, byte[]> adapter)
        {
            _serverCommandAdapter = adapter;
        }

        public ServerCommand Handle(ServerMessage message) => _serverCommandAdapter.Adapt(message.Data);
    }
}
