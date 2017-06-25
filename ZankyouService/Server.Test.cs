using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZankyouService
{
    using NUnit.Framework;
    using System.Net.Sockets;
    using System.Threading;

    [TestFixture]
    public class ServerTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Server server = new Server();
        }

        [Test()]
        public void TestConnect()
        {
            Socket client = new Socket(SocketType.Stream, ProtocolType.IP);
            client.Connect("127.0.0.1", 12345);
            client.Close();
        }

        [Test()]
        public void TestMultiConnect()
        {
            for (int i = 0; i < 10; i++)
            {
                Socket client = new Socket(SocketType.Stream, ProtocolType.IP);
                client.Connect("127.0.0.1", 12345);
            }
        }

        [Test()]
        public void TestMessage()
        {
            Socket client = new Socket(SocketType.Stream, ProtocolType.IP);
            client.Connect("127.0.0.1", 12345);
            client.Send(Encoding.ASCII.GetBytes("hello world"));
            client.Close();
        }

        [Test()]
        public void TestMuliClientMessages()
        {
            for (int i = 0; i < 10; i++)
            {
                Socket client = new Socket(SocketType.Stream, ProtocolType.IP);
                client.Connect("127.0.0.1", 12345);
                client.Send(Encoding.ASCII.GetBytes("hello world"));
            }
        }
    }
}
