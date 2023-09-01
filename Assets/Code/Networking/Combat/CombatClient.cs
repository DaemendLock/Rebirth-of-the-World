using Core.Networking.Combat;
using System;
using System.Net.Sockets;

namespace Core.Net.Combat
{
    public class CombatClient : IDisposable
    {
        private TcpClient _client;

        public CombatClient(string ip, int port)
        {
            
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

        public void SendInput(InputData input)
        {
            throw new NotImplementedException();
        }

        private void CreateUnit(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void BeginCast(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void StopCast(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void MoveUnit(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void StopMovement(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void Kill(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void Resurect(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void AllowControl(byte[] data)
        {
            throw new NotImplementedException();
        }

        private void DisallowControl(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
