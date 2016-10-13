using GraphLabs.DomainModel.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Extensions
{
    /// <summary> Расширения для GraphLabsContext </summary>
    public static class EntityQueryExtensions
    {
        /// <summary> Поиск сущности (ошибка, если не нашёл) </summary>
        /// <exception cref="EntityNotFoundException">Сущность с заданным ключом не найдена</exception>
        [NotNull]
        public static TEntity Get<TEntity>(this IEntityQuery query, object keyValue)
            where TEntity : AbstractEntity
        {
            var entity = query.Find<TEntity>(keyValue);
            if (entity == null)
                throw new EntityNotFoundException(typeof(TEntity), new [] { keyValue });

            return entity;
        }

        /// <summary> Поиск сущности (null, если не нашёл) </summary>
        [CanBeNull]
        public static TEntity Find<TEntity>(this IEntityQuery query, object keyValue)
            where TEntity : AbstractEntity
        {
            return query.Find<TEntity>(new [] {keyValue});
        }
    }
}