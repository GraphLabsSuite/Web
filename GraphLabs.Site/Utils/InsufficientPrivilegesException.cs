using System;

namespace GraphLabs.Site.Utils
{
    /// <summary> Исключение безопасноси: недостаточно прав на выполнение действия. </summary>
    public class InsufficientPrivilegesException : Exception
    {
        /// <summary> Ctor. </summary>
        public InsufficientPrivilegesException(string message) : base(message)
        {
        }
    }
}