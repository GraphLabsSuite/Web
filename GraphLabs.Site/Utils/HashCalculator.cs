using System;
using System.Security.Cryptography;
using System.Text;

namespace GraphLabs.Site.Utils
{
    /// <summary> Класс для вычисления хеш-суммы паролей </summary>
    public static class HashCalculator
    {
        /// <summary> Генерируем хеш по массиву байт и некоему добавочному шуму </summary>
        public static string GenerateSaltedHash(string plainText, string saltBase64String)
        {
            var bytes = Encoding.UTF8.GetBytes(plainText);
            var salt = Convert.FromBase64String(saltBase64String);

            HashAlgorithm algorithm = new SHA256Managed();

            var plainTextWithSaltBytes = new byte[bytes.Length + salt.Length];

            Array.Copy(bytes, plainTextWithSaltBytes, bytes.Length);
            Array.Copy(salt, 0, plainTextWithSaltBytes, bytes.Length, salt.Length);

            var hash = algorithm.ComputeHash(plainTextWithSaltBytes);
            return Convert.ToBase64String(hash);
        }

        /// <summary> Случайный шум </summary>
        public static string GenerateRandomSalt()
        {
            var rnd = new Random();
            var length = rnd.Next(5, 10);
            
            var salt = new byte[length];
            rnd.NextBytes(salt);

            return Convert.ToBase64String(salt);
        }
    }
}