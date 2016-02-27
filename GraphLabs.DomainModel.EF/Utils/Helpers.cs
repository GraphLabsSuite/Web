using System.IO;

namespace GraphLabs.DomainModel.EF.Utils
{
    /// <summary> Класс со всякой вспомогательной мелочью </summary>
    /// <remarks> Быть может, потом перерастёт во что-то более конкретное </remarks>
    public static class Helpers
    {
        /// <summary> Читает входной поток до самого конца </summary>
        public static byte[] ReadToEnd(this Stream input)
        {
            const int BUFFER_SIZE = 16 * 1024;

            var buffer = new byte[BUFFER_SIZE];
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
