using Networking.Utils;

namespace Assets.Sources.Temp
{
    public class AutoAcceptClient : Client
    {
        public override void Dispose()
        {
            //throw new NotImplementedException();
        }

        public override void SendRequest(byte[] data)
        {
            Adapters.Combat.ServerCommandsAdapter.HandleCommand(data);
        }
    }
}
