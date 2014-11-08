using System.Diagnostics.Contracts;
using System.Net;

namespace GraphLabs.Site.Utils
{
    /// <summary> Вспомогательные методы для работы с IP-адресами </summary>
    public static class IpHelper
    {
        /// <summary> Поверяет, что переданная строка - корректный IP адрес </summary>
        [Pure]
        public static bool CheckIsValidIP(string address)
        {
            IPAddress ipAddress;
            return IPAddress.TryParse(address, out ipAddress) && 
                (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ||
                 ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
        }
    }
}
