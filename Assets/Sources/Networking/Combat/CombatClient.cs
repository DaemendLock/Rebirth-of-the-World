using System;
using System.Net.Sockets;

namespace Networking.ClientCombat
{
    public class CombatClient : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _source;

        public CombatClient(string ip, int port)
        {
            _client = new TcpClient(ip, port);
        }

        ~CombatClient()
        {
            _client?.Close();
        }

        public bool CheckConnection()
        {
            return false;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        public void RequestSync()
        {

        }
        /*
        public void SendInput(InputData input)
        {
            throw new NotImplementedException();
        }*/
    }
}
