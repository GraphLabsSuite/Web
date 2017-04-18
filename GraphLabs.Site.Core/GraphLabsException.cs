using System;
using System.Diagnostics.Contracts;

namespace GraphLabs.Site.Core
{
    /// <summary> Исключение GraphLabs </summary>
    public class GraphLabsException : Exception
    {
        private Exception innerException;

        public GraphLabsException(Exception innerException)
        {
            Contract.Requires<ArgumentNullException>(innerException != null);
            this.innerException = innerException;
        }

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
