using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Networking {

    public class ServerManager {
        private const string SERVER = "localhost";
        private const int PORT = 27122;

        public static event Action<AccountAccessResponse> AccountAccessFigured;
        public static event Action<Unit, float, float> ServerUnitMoveCommand;


        private static Account _account;
        private static NetworkModule _connection = null;

        public static bool TryConnect() {
            if (_connection != null) {
                return false;
            }
            try {
                _connection = new(SERVER, PORT);
            } catch (Exception) {
                return false;
            }
            _connection.ServerMessageRecived += ResponseResolver.HandleResponse;
            return true;
        }

        public static void Disonnect() {
            if (_connection == null) {
                return;
            }
            _connection.ServerMessageRecived -= ResponseResolver.HandleResponse;
            _connection.Close();
            _connection = null;
        }

        public static void LoginAttempt(string login, string password) => _connection.SendRequest(Request.MakeAccountAccessRequest( _connection.NextPacketId, DataType.LOGIN_DATA, login, password));

        public static void RegisterAccount(string login, string password) => _connection.SendRequest(Request.MakeAccountAccessRequest(_connection.NextPacketId, DataType.REGISTER_DATA, login, password));

        //public static void RequestAccountData(int UID, DataType type, byte blockNumber) => _connection.(UID, type, blockNumber);

        public static void InfoLogout() {
            if (_connection == null)
                return;
            _connection.SendRequest(Request.MakeLogoutRequest());
        }

        public static void SendAccountAccessStatus(AccountAccessResponse response) {
            AccountAccessFigured?.Invoke(response);
        }

        public static Account ActiveAccount => _connection != null ? _connection.Account : null;

        private static class ResponseResolver {
            public static void HandleResponse(Response response) {
                switch (response.Type) {
                    case DataType.CONNECTION_CHECK:
                        _connection.SendRequest(Request.MakePingRequest(_connection.NextPacketId));
                        break;
                    case DataType.LOGIN_DATA:
                        AccountAccessFigured?.Invoke(new AccountAccessResponse(response));
                        break;
                    case DataType.DISCONNECT:
                        _connection.Close();
                        _connection = null;
                        break;
                    default:
                        break;
                    
                }
            }

            
        } 

    }

}