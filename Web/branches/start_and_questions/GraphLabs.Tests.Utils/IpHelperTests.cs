using System.Diagnostics;
using System.Linq;
using GraphLabs.Site.Utils;
using NUnit.Framework;

namespace GraphLabs.Tests.Utils
{
    /// <summary> Проверим <see cref="IpHelper"/> </summary>
    [TestFixture]
    public class IpHelperTests
    {
        /// <summary> Проверка корректности IPv4 </summary>
        [Test]
        public void TestIPv4Check()
        {
            Assert.IsTrue(IpHelper.CheckIsValidIP("172.16.0.10"));
            Assert.IsTrue(IpHelper.CheckIsValidIP("192.168.10.10"));
            Assert.IsTrue(IpHelper.CheckIsValidIP("192.168"));
            Assert.IsTrue(IpHelper.CheckIsValidIP("192.168.255.0"));
            Assert.IsTrue(IpHelper.CheckIsValidIP("192.168.10.0"));
            Assert.IsTrue(IpHelper.CheckIsValidIP("255.255.255.255"));
            Assert.IsTrue(IpHelper.CheckIsValidIP("::1"));
            Assert.IsFalse(IpHelper.CheckIsValidIP("255.255.255.asdf"));
        }
    }
}
