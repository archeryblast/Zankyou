﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ZankyouService
{
    class ServerMessage
    {
        public string method;
        public object data;
    }

    public class Server
    {
        private TcpListener serverListener;
        
        /// <summary>
        /// Creates a sockets that listens on the specified port,
        /// configured in the user configuration.
        /// </summary>
        public Server(string ipAddress = "192.168.0.7", int port = 12345)
        {
            var localHost = IPAddress.Parse(ipAddress);
            serverListener = new TcpListener(localHost, port);
            serverListener.Start();
            serverListener.BeginAcceptSocket(this.AcceptClient, serverListener);
        }

        /// <summary>
        /// Accepts a client asynchronously, allowing the 
        /// TcpListener to continue listening for new clients.
        /// </summary>
        /// <param name="ar">The asychronous response representing the tcp listener</param>
        private void AcceptClient(IAsyncResult ar)
        {
            var listener = (TcpListener) ar.AsyncState;
            if (listener != null)
            {
                try
                {
                    Socket clientSocket = listener.EndAcceptSocket(ar);
                    StartReceive(clientSocket);
                    
                } catch
                {
                    Console.WriteLine("Error with client connection");
                }
                Console.WriteLine("Client successfully connected");
                listener.BeginAcceptSocket(this.AcceptClient, listener);
            }
        }

        /// <summary>
        /// Waits for messages from client asynchronously.
        /// </summary>
        /// <param name="clientSocket">The client socket that is receiving messages.</param>
        private void StartReceive(Socket clientSocket)
        {
            byte[] buffer = new byte[200];
            try
            {
                clientSocket.BeginReceive(buffer, 0, 200, SocketFlags.None, (state) =>
                {
                    try
                    {
                        if (clientSocket.Connected)
                        {
                            int bytesReceived = clientSocket.EndReceive(state);
                            if (bytesReceived != 0)
                            {
                                string message = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                                Console.WriteLine(message);
                                switch (message)
                                {
                                    case "GetSystemInfo":
                                        {
                                            ServerMessage clientMessage = new ServerMessage() { method = "GetSystemInfo", data = Monitor.GetSystemUsage() };
                                            string result = Newtonsoft.Json.JsonConvert.SerializeObject(clientMessage);
                                            // sends data to client
                                            clientSocket.Send(Encoding.ASCII.GetBytes(result));
                                            break;
                                        }
                                    case "GetProcessInfo":
                                        {
                                            ServerMessage clientMessage = new ServerMessage() { method = "GetSystemInfo", data = Monitor.GetSystemProcesses() };
                                            string result = Newtonsoft.Json.JsonConvert.SerializeObject(clientMessage);
                                            // sends data to client
                                            clientSocket.Send(Encoding.ASCII.GetBytes(result));
                                            break;
                                        }
                                    default:
                                        Console.WriteLine("Message: {{0}} unrecognized", message);
                                        break;
                                }
                                StartReceive(clientSocket);
                            }
                        }
                    } catch
                    {
                        Console.WriteLine("Error with connected client");
                    }
                }, clientSocket);
            } catch
            {
                Console.WriteLine("Error with connected client");
            }
        }
    }
}
