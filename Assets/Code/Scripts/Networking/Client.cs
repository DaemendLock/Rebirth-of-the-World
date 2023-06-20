using AccountsData;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Networking {
    public sealed class Client {
        private const int PORT = 27122;
        private const string SERVER = "localhost";

        public event Action<Response> ResponseReceived;
        private NetworkModule _networkModule;
        private Account _account;

        public Client() {
            _networkModule = new(SERVER, PORT);
            _networkModule.ServerMessageRecived += HandleServerResponse;
        }

        public Account Account => _account;

        public void SendAccountLoginRequest(string login, string password, bool register = false) {
            _networkModule.SendRequest(Request.MakeAccountAccessRequest(_networkModule.NextPacketId, register ? DataType.REGISTER_DATA : DataType.LOGIN_DATA, login, password));
        }

        public void SendLogout() {
            _networkModule.SendRequest(Request.MakeLogoutRequest(_networkModule.NextPacketId));
        }

        public void Close() {
            SendLogout();
            _networkModule.Close();
        }

        private void HandleServerResponse(Response response) {
            switch (response.Type) {
                case DataType.CONNECTION_CHECK:
                    HandlePing(response);
                    break;
                case DataType.LOGOUT:
                    Close();
                    break;
                default:

                    break;
            }
        }

        private void HandlePing(Response serverMsg) {
            long data = BitConverter.ToInt64(serverMsg.Data, Response.HEADER_SIZE);
            Debug.Log(DateTime.UnixEpoch.Millisecond - data);
            _networkModule.SendRequest(Request.MakePingRequest(_networkModule.NextPacketId));
        }

        private void AccountResponded(AccountAccessResponse response) {
            if (response.Success) {
                _account = new Account(response.UID);
            }
            
        }

        private static byte[] GetBytesForString(string toConver) {
            return System.Text.Encoding.ASCII.GetBytes(toConver);
        }
    }
}
