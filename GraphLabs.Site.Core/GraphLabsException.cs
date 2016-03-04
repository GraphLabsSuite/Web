using System;

namespace GraphLabs.Site.Core
{
    /// <summary> Исключение GraphLabs </summary>
    public class GraphLabsException : Exception
    {
        /// <summary> Исключение GraphLabs </summary>
        public GraphLabsException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        /// <summary> Исключение GraphLabs </summary>
        public GraphLabsException(Exception innerException, string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}
