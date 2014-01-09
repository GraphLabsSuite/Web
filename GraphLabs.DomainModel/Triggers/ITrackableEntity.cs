using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace GraphLabs.DomainModel
{
    /// <summary> Отслеживаемые сущности </summary>
    public interface ITrackableEntity
    {
        /// <summary> Перед сохранением новой сущности в базу </summary>
        void OnInsert();

        /// <summary> Перед сохранением изменённой сущности в базу </summary>
        void OnChange(DbEntityEntry entry);

        /// <summary> Валидация </summary>
        IEnumerable<DbValidationError> OnValidating(DbEntityEntry entityEntry);
    }
}