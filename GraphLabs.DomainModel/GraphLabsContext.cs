using System;
using log4net;

namespace GraphLabs.DomainModel
{
    public partial class GraphLabsContext
    {
        private static ILog _log = LogManager.GetLogger(typeof(GraphLabsContext));

        /// <summary> Для тестов: позволяет подсунуть свою строку подключения </summary>
        /// <param name="connectionStringName">Имя строки подключения в конфиге</param>
        internal GraphLabsContext(string connectionStringName)
            : base(string.Format("name={0}", connectionStringName))
        {
        }

        private bool _isDisposed = false;

        /// <summary>
        /// Disposes the context. The underlying <see cref="T:System.Data.Entity.Core.Objects.ObjectContext"/> is also disposed if it was created
        ///             is by this context or ownership was passed to this context when this context was created.
        ///             The connection to the database (<see cref="T:System.Data.Common.DbConnection"/> object) is also disposed if it was created
        ///             is by this context or ownership was passed to this context when this context was created.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        ///             </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                if (ChangeTracker.HasChanges())
                    _log.Warn("В уничтожаемом контексте остались несохранённые данные.");
            }
            _isDisposed = true;
            base.Dispose(disposing);
        }
    }
}
