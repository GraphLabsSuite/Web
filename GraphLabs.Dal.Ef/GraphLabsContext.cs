using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using GraphLabs.Dal.Ef.Infrastructure;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Infrastructure;
using log4net;
using Microsoft.Practices.ObjectBuilder2;

namespace GraphLabs.Dal.Ef
{
    /// <summary> EF-контекст </summary>
    public partial class GraphLabsContext

    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(GraphLabsContext));

        /// <summary> Для тестов: позволяет подсунуть свою строку подключения </summary>
        /// <param name="connectionStringName">Имя строки подключения в конфиге</param>
        internal GraphLabsContext(string connectionStringName)
            : base(string.Format("name={0}", connectionStringName))
        {
            Configuration.LazyLoadingEnabled = true;
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
            var changedEntries = ChangeTracker.Entries();
            foreach (var changedEntry in changedEntries)
            {
                switch (changedEntry.State)
                {
                    case EntityState.Added:
                        changedEntry.State = EntityState.Detached;
                        break;

                    case EntityState.Deleted:
                    case EntityState.Modified:
                        var propertyNames = changedEntry.CurrentValues.PropertyNames;
                        foreach (var propertyName in propertyNames)
                        {
                            var originalValue = changedEntry.OriginalValues[propertyName];
                            if (changedEntry.CurrentValues[propertyName] != originalValue)
                            {
                                var property = changedEntry.Property(propertyName);
                                property.CurrentValue = originalValue;
                                property.IsModified = false;
                            }
                        }
                        changedEntry.State = EntityState.Unchanged;
                        break;
                    
                    case EntityState.Unchanged:
                        break;
                    
                    default:
                        throw new NotSupportedException(string.Format("В списке изменённых оказалась сущность в состоянии {0}.", changedEntry.State));
                }
            }
        }

        #region Triggers

        /// <summary> Saves all changes made in this context to the underlying database. </summary>
        public override int SaveChanges()
        {
            var trackables = ChangeTracker.Entries<ITrackableEntity>().ToArray();

            // added
            foreach (var item in trackables.Where(t => t.State == EntityState.Added))
            {
                item.Entity.OnInsert();
            }
            // modified
            foreach (var item in trackables.Where(t => t.State == EntityState.Modified))
            {
                item.Entity.OnChange(new EntityChange(item));
            }

            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessage = string.Join(Environment.NewLine, ex.EntityValidationErrors
                    .SelectMany(err => err.ValidationErrors.Select(e => e.ErrorMessage)));
                throw new GraphLabsValidationException(ex, errorMessage);
            }
            catch (DbUpdateException e)
            {
                throw new GraphLabsDbUpdateException(e.InnerException);
            }

        }

        /// <summary> Extension point allowing the user to customize validation of an entity or filter out validation results. </summary>
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var trackableEntity = entityEntry.Entity as ITrackableEntity;
            if (trackableEntity != null)
            {
                var errors = trackableEntity
                    .OnValidating()
                    .Select(err => new DbValidationError(err.Property, err.Error));

                return new DbEntityValidationResult(entityEntry, errors);
            }

            return new DbEntityValidationResult(entityEntry, Enumerable.Empty<DbValidationError>());
        }

        #endregion
    }
}
