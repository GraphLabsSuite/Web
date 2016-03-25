using GraphLabs.DomainModel.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Расширения для GraphLabsContext </summary>
    public static class EntityQueryExtensions
    {
        /// <summary> Поиск сущности (ошибка, если не нашёл) </summary>
        /// <exception cref="EntityNotFoundException">Сущность с заданным ключом не найдена</exception>
        [NotNull]
        public static TEntity Get<TEntity>(this IEntityQuery query, params object[] keyValues)
            where TEntity : AbstractEntity
        {
            var entity = query.Find<TEntity>();
            if (entity == null)
                throw new EntityNotFoundException(typeof(TEntity), keyValues);

            return entity;
        }
    }
}