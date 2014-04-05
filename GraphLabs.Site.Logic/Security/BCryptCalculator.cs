using CryptSharp;
using JetBrains.Annotations;

namespace GraphLabs.Site.Logic.Security
{
    /// <summary> Класс для вычисления хеш-суммы паролей </summary>
    [UsedImplicitly]
    public class BCryptCalculator : IHashCalculator
    {
        /// <summary> Генерирует хеш </summary>
        public string Crypt(string text)
        {
            return Crypter.Blowfish.Crypt(text);
        }

        /// <summary> Проверка </summary>
        public bool Verify(string text, string hash)
        {
            return Crypter.Blowfish.Crypt(text, hash) == hash;
        }
    }
}