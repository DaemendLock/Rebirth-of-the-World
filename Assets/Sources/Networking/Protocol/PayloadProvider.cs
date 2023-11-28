using System.IO;

namespace Networking.Protocol
{
    public interface PayloadProvider
    {
        public void WriteBytes(Stream target);
    }
}
