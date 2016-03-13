using GraphLabs.DomainModel;

namespace GraphLabs.Dal.Ef.Extensions
{
    /// <summary> Класс расширений для сессий </summary>
    public static class SessionExtensions
    {
        private const int SESSION_DEACTIVATION_TIMEOUT_MINUTES = 5;

        /// <summary> Сессия активна? </summary>
        public static bool IsActive(this Session session)
        {
            return (session.LastAction - session.CreationTime).Minutes < SESSION_DEACTIVATION_TIMEOUT_MINUTES;
        }
    }
}
