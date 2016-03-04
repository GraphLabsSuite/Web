using System.Collections.Generic;

namespace GraphLabs.DomainModel.Infrastructure
{
    /// <summary> Отслеживаемые сущности </summary>
    public interface ITrackableEntity
    {
        /// <summary> Перед сохранением новой сущности в базу </summary>
        void OnInsert();

        /// <summary> Перед сохранением изменённой сущности в базу </summary>
        void OnChange(IEntityChange change);

        /// <summary> Валидация </summary>
        IEnumerable<EntityValidationError> OnValidating();
    }
}