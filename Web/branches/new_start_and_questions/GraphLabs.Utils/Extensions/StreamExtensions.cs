using System;
using System.IO;

namespace GraphLabs.Site.Utils.Extensions
{
    /// <summary> Расширения для потоков </summary>
    public static class StreamExtensions
    {
        /// <summary> Читает входной поток до самого конца </summary>
        public static byte[] ReadToEnd(this Stream input)
        {
            const int bufferSize = 16 * 1024;

            var buffer = new byte[bufferSize];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
