using System;

namespace Networking.Utils
{
    public abstract class Client : IDisposable
    {

        public abstract void SendRequest(byte[] data);

        public abstract void Dispose();
    }
}
