using System;
using System.Net;
using System.Net.NetworkInformation;

namespace ZankyouService
{
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void getAvailbeHostNamesTest()
        {
            List<IPAddress> list = Configuration.GetAvailbeIPAddresses();
            foreach (var l in list)
            {
                Console.WriteLine(l.ToString());
            }
        }

    }
}
