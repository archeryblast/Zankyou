using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Json;

namespace Zankyou
{
    class ServerMessage {
        public string method;
        public object data;
    }
    class Client
    {
        private Socket socket;
        /// <summary>
        /// Constructor to create a client to a remote server.
        /// </summary>
        /// <param name="hostname">hostname or ip of the remote server</param>
        /// <param name="port">port number of the remote server, 12345 is the default port</param>
        public Client(string hostname, int port = 12345)
        {
            socket = new Socket(SocketType.Stream, ProtocolType.IP);
            socket.Connect(hostname, port);
            StartReceive();
        }

        private void StartReceive()
        {
            byte[] buffer = new byte[200];
            socket.BeginReceive(buffer, 0, 200, SocketFlags.None,
                (state) =>
                {
                    if (socket.Connected)
                    {
                        int bytesReceived = socket.EndReceive(state);
                        if (bytesReceived != 0)
                        {
                            string jsonMessage = Encoding.ASCII.GetString(buffer);
                            JsonValue value = JsonValue.Parse(jsonMessage.Substring(0, bytesReceived));
                            ServerMessage message = new ServerMessage()
                            {
                                method = value["method"],
                                data = value["data"]
                            };
                            HandleServerMessage(message);
                        }
                        StartReceive();
                    }
                    else
                    {
                        Console.WriteLine("Lost Connection to host");
                    }
                }, socket);
        }

        /// <summary>
        /// Handles server messages.
        /// </summary>
        /// <param name="message">message to be handled</param>
        private void HandleServerMessage(ServerMessage message)
        {
            switch (message.method)
            {
                case "GetSystemInfo":
                    Console.WriteLine(message.data);
                    break;
                default:
                    Console.WriteLine("Message unrecognized: " + message);
                    break;
            }
        }

        /// <summary>
        /// Sends messages to the server
        /// </summary>
        /// <param name="message">message (string) to be sent to server</param>
        public void SendMessage(string message)
        {
            socket.Send(Encoding.ASCII.GetBytes(message));
        }
    }
}
