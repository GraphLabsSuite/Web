using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace GraphLabs.DomainModel
{
    public partial class GraphLabsContext
    {
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
                item.Entity.OnChange(item);
            }

            return base.SaveChanges();
        }

        /// <summary> Extension point allowing the user to customize validation of an entity or filter out validation results. </summary>
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var trackableEntity = entityEntry.Entity as ITrackableEntity;
            if (trackableEntity != null)
            {
                return new DbEntityValidationResult(entityEntry, trackableEntity.OnValidating(entityEntry));
            }

            return new DbEntityValidationResult(entityEntry, Enumerable.Empty<DbValidationError>());
        }
    }
}
