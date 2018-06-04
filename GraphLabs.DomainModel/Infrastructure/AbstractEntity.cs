using System.Collections.Generic;
using System.Linq;

namespace GraphLabs.DomainModel.Infrastructure
{
    /// <summary> Абстрактная сущность </summary>
    public abstract class AbstractEntity : ITrackableEntity
    {
        /// <summary> Перед сохранением новой сущности в базу </summary>
        public virtual void OnInsert()
        {
        }

        /// <summary> Перед сохранением изменённой сущности в базу </summary>
        public virtual void OnChange(IEntityChange change)
        {
            Guard.IsNotNull(nameof(change), change);
        }

        /// <summary> Валидация </summary>
        /// <remarks> Для переопределения валидации конеретных сущностей перекрывайте OnEntityValidating</remarks>
        public IEnumerable<EntityValidationError> OnValidating()
        {
            var result = OnEntityValidating();
            Guard.IsNotNull(nameof(result), result);
            return result;
        }

        /// <summary> Валидация </summary>
        public virtual IEnumerable<EntityValidationError> OnEntityValidating()
        {
            return Enumerable.Empty<EntityValidationError>();
        }
    }
}
