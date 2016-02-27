using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace GraphLabs.DomainModel.EF
{
    /// <summary> Абстрактная сущность </summary>
    public abstract class AbstractEntity : ITrackableEntity
    {
        /// <summary> Перед сохранением новой сущности в базу </summary>
        public virtual void OnInsert()
        {
        }

        /// <summary> Перед сохранением изменённой сущности в базу </summary>
        public virtual void OnChange(DbEntityEntry entry)
        {
        }

        /// <summary> Валидация </summary>
        /// <remarks> Для переопределения валидации конеретных сущностей перекрывайте OnEntityValidating</remarks>
        public IEnumerable<DbValidationError> OnValidating(DbEntityEntry entityEntry)
        {
            return OnEntityValidating(entityEntry);
        }

        /// <summary> Валидация </summary>
        public abstract IEnumerable<DbValidationError> OnEntityValidating(DbEntityEntry entityEntry);
    }
}
