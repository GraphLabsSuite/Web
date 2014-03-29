namespace GraphLabs.Site.Logic.Security
{
    /// <summary> Вычислятор хеш-суммы паролей </summary>
    public interface IHashCalculator
    {
        /// <summary> Генерирует соль </summary>
        string GenerateSalt();

        /// <summary> Генерирует хеш </summary>
        string Crypt(string text);

        /// <summary> Проверка </summary>
        bool Verify(string text, string hash);
    }
}