using AccountsData;
using System;

namespace Networking {

    public class ServerManager {

        public static event Action<Response> ServerMessageRecived;
        public static event Action<ResponsableRequest> RequestGotResponse;
        public static event Action<AccountAccessResponse> AccountAccessFigured;

        private static Client connection;

        public static bool TryConnect() {
            if (connection != null) {
                return false;
            }
            try {
                connection = Client.Instantiate(null) as Client;
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public static void Disonnect() {
            if (connection == null) {
                return;
            }
            connection.Close();
            connection = null;
        }

        public static ResponsableRequest LoginAttempt(string login, string password) => Client.AttemptAccountLogin(connection, login, password);

        public static ResponsableRequest RegisterAccount(string login, string password) => Client.AttemptAccountLogin(connection, login, password, true);

        public static ResponsableRequest RequestAccountData(int UID, DataType type, byte blockNumber) => Client.RequestAccountData(connection, UID, type, blockNumber);

        public static void InfoLogout() {
            if (connection == null)
                return;
            Client.InfoLogout(connection);
        }

        public static void SendToServer(string context) {
            context += '\0';
            Client.MakeSingleRequest(connection, context);
        }

        public static void InformServerResponsed(Response request) {
            ServerMessageRecived?.Invoke(request);
        }

        public static void RespondeRequest(ResponsableRequest request) {
            RequestGotResponse?.Invoke(request);
        }

        public static void SendAccountAccessStatus(AccountAccessResponse response) {
            AccountAccessFigured?.Invoke(response);
        }

        public static Account ActiveAccount => connection ? connection.Account : null;

    }

}