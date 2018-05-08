using System;
using System.Diagnostics.Contracts;

namespace GraphLabs.Dal.Ef.Repositories
{
    /// <summary> Базовый репозиторий </summary>
    public abstract class RepositoryBase : IDisposable
    {
        /// <summary> Контекст GraphLabs </summary>
        protected GraphLabsContext Context { get; private set; }
        private bool _disposed;

        /// <summary> Базовый репозиторий </summary>
        protected RepositoryBase(GraphLabsContext context)
        {
            Contract.Requires(context != null);
            Guard.Guard.IsNotNull(context, nameof(context)); //*

            _disposed = false;
            Context = context;
        }

        /// <summary> Проверяет, не уничтожен ли репозиторий </summary>
        protected void CheckNotDisposed()
        {
            if (_disposed)
                throw new InvalidOperationException("Для репозитория уже был вызван деструктор.");
        }

        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public virtual void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }
    }
}
