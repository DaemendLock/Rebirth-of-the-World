using AccountsData;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Networking {
    public sealed class Client : MonoBehaviour {

        private static readonly int port = 27122;
        private static readonly string server = "localhost";
        private static readonly int BUFFER_SIZE = 128;
        private const int DATA_REQUEST_SIZE = 8;

        private byte _requestId = 1;
        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _inputThread;
        private bool _stop = true;
        private Account _account;

        private byte[] _reciveBuffer = new byte[BUFFER_SIZE];



        private SynchronizationContext _context = SynchronizationContext.Current ?? new SynchronizationContext();

        private void Start() {
            _client = new TcpClient(server, port);
            _stream = _client.GetStream();
            _inputThread = new Thread(ListenServerResponse);
            _inputThread.Start();
            ServerManager.ServerMessageRecived += HandleServerResponse;
            ServerManager.AccountAccessFigured += AccountResponded;
            ServerManager.RequestGotResponse += HandleRequestResponse;
            DontDestroyOnLoad(this);
        }

        private void AccountResponded(AccountAccessResponse response) {
            if (response.Success) {
                _account = new Account(response.UID);
            }
        }

        public Account Account => _account;

        public static ResponsableRequest AttemptAccountLogin(Client connection, string login, string password, bool register = false) {
            ResponsableRequest request = new(new byte[] { connection._requestId, (byte) (register ? RequestType.REGISTER_ACOUNT : RequestType.GET_ACCOUNT_ACCESS) });
            SendRequest(connection, request);
            MakeMultiRequest(connection, new List<string>() { login, password });
            return request;
        }

        public static ResponsableRequest RequestAccountData(Client connection, int UID, DataType type, byte blockIndex) {
            byte[] data = new byte[DATA_REQUEST_SIZE] { connection._requestId, (byte) RequestType.GET_ACCOUNT_DATA, 0, 0, 0, 0, (byte) type, blockIndex };
            byte[] bytes = BitConverter.GetBytes(UID);
            bytes.CopyTo(data, 2);
            ResponsableRequest request = new(data);
            SendRequest(connection, request);
            return request;
        }

        public static void InfoLogout(Client connection) {
            SendRequest(connection, new byte[] { connection._requestId, (byte) RequestType.LOGOUT });
        }

        public static void MakeMultiRequestByte(Client connection, List<byte[]> requests) {
            foreach (byte[] request in requests) {
                SendRequest(connection, request);
            }
        }

        public static void MakeMultiRequest(Client connection, List<string> requests) {
            foreach (string request in requests) {
                SendRequest(connection, request);
            }
        }

        public static void MakeSingleRequest(Client connection, string request) {
            SendRequest(connection, request);
        }

        public static void Close(Client connection) {
            connection.Close();
        }

        public void Close() {
            UnityEngine.Debug.Log("Connection closed");
            _inputThread.Abort();
            _stop = false;
            _stream.Close();
            _client.Close();
        }

        private void HandleServerResponse(Response response) {
            byte[] data = response.Data;

            switch (response.Type) {
                case ResponseType.PING_CLIENT:
                    long res = 0;
                    for (int i = 0; i < 8; i++) {
                        res = res << 8;
                        res += data[i];
                    }
                    HandlePingRequest(res);
                    break;
                case ResponseType.CONNECTION_CLOSED:
                    Close(this);
                    break;
                default:

                    break;
            }
        }

        private void HandleRequestResponse(ResponsableRequest request) {
            Response response = request.Response;
            byte[] data = request.Response.Data;
            UnityEngine.Debug.Log("Handling responded request");
            switch (response.Type) {
                case ResponseType.ACCOUNT_ACCESS_APPROVED:
                    UnityEngine.Debug.Log("-> Account access");
                    _account = new Account(BitConverter.ToInt32(data, 3));
                    ServerManager.SendAccountAccessStatus(new AccountAccessResponse(data));
                    break;
                case ResponseType.REGISTER_APPROVED:
                    UnityEngine.Debug.Log("-> Register access");
                    _account = new Account(BitConverter.ToInt32(data, 3));
                    ServerManager.SendAccountAccessStatus(new AccountAccessResponse(data));
                    break;
                case ResponseType.ACCOUNT_DATA:
                    UnityEngine.Debug.Log("-> Account data");
                    _account.Data.HandleData(request);
                    break;
                default:

                    break;
            }
        }

        private void HandlePingRequest(long serverTime) {
            List<byte> response = new() { _requestId, (byte) ResponseType.PING_CLIENT };
            response.AddRange(GetBytesForString(server.ToString()));
            SendRequest(this, response.ToArray());
        }

        private void ListenServerResponse() {
            while (_stop) {
                Response res = new Response(GetByteResponse(this));
                /*string print = "";
                foreach (byte b in res.Data) {
                    print += b +" ";
                }*/
                ServerManager.InformServerResponsed(res);
            }
        }

        private static void SendRequest(Client connection, string request) {
            SendRequest(connection, GetBytesForString(request + '\0'));
        }

        private static void SendRequest(Client connection, Request request) {
            SendRequest(connection, request.Data);
        }

        private static void SendRequest(Client connection, byte[] data) {
            connection._requestId++;
            if (connection._requestId == 0) {
                connection._requestId = 1;
            }
            if (data.Length != BUFFER_SIZE) {
                byte[] buffer = data;
                data = new byte[BUFFER_SIZE];
                for (int i = 0; i < buffer.Length; i++) {
                    data[i] = buffer[i];
                }
            }
            connection._stream.Write(data, 0, BUFFER_SIZE);
            connection._stream.Flush();
        }

        private static byte[] GetByteResponse(Client connection) {
            byte[] data = new byte[BUFFER_SIZE];
            connection._stream.Read(data, offset: 0, data.Length);
            return data;
        }

        private static byte[] GetBytesForString(string toConver) {
            return System.Text.Encoding.ASCII.GetBytes(toConver);
        }
    }
}
