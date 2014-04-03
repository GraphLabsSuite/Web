using System;
using System.Data.Entity;
using GraphLabs.DomainModel.Extensions;
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

        /// <summary> Откатывает изменения. </summary>
        public void RollbackChanges()
        {
            ChangeTracker.DetectChanges();
            var changedEntities = ChangeTracker.Entries();
            foreach (var changedEntity in changedEntities)
            {
                var propertyNames = changedEntity.CurrentValues.PropertyNames;
                foreach (var propertyName in propertyNames)
                {
                    var originalValue = changedEntity.OriginalValues[propertyName];
                    if (changedEntity.CurrentValues[propertyName] != originalValue)
                    {
                        var property = changedEntity.Property(propertyName);
                        property.CurrentValue = originalValue;
                        property.IsModified = false;
                    }
                }
                changedEntity.State = EntityState.Unchanged; // а что будет с только что созданными?
            }
        }

    }
}
