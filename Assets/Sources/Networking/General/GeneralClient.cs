namespace Networking.General
{
    public class GeneralClient
    {
        private static readonly string login = "admin";
        private static readonly string password = "admin";
        private string _ip;
        private int _port;

        public GeneralClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }
    }
}
