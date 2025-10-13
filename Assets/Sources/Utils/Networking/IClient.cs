using System.Threading.Tasks;

using Utils.Networking;

namespace UtilsUnity.Networking
{
    public interface IClient
    {
        bool HasMessages { get; }

        Task Connect();

        void Disconnect();

        void SendRequest(Request request);

        Utils.ByteHelper.ServerMessage NextMessage();
    }
}
