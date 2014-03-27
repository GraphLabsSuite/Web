namespace GraphLabs.Site.Logic.Security
{
    /// <summary> ���������� ���-����� ������� </summary>
    public interface IHashCalculator
    {
        /// <summary> ���������� ���� </summary>
        string GenerateSalt();

        /// <summary> ���������� ��� </summary>
        string Crypt(string text);

        /// <summary> �������� </summary>
        bool Verify(string text, string hash);
    }
}