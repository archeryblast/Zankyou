using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace ZankyouService
{
    public static class Configuration
    {
        private static int port;
        private static IPAddress hostname;

        public static int Port { get { return port; } set { port = value; } }
        public static string Hostname
        {
            get
            {
                return hostname.ToString();
            }
        }

        public static List<IPAddress> GetAvailbeIPAddresses()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            var result = host.AddressList.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToList();
            return result;
        }

        public static void SetHostname(IPAddress selected)
        {
            hostname = selected;
        }

    }
}
