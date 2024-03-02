namespace Assets.Sources.Temp
{
    public class AutoAcceptClient : Networking.Utils.Client
    {
        public override void Dispose()
        {
            //throw new NotImplementedException();
        }

        public override void SendRequest(byte[] data) => Server.Combat.CombatServer.Handle(data);
    }
}
