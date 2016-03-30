using System.Linq;
using GraphLabs.DomainModel.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel
{
    /// <summary> Универсальный запрос сущности </summary>
    public interface IEntityQuery
    {
        /// <summary> Запрос сущностей </summary>
        [NotNull]
        IQueryable<TEntity> OfEntities<TEntity>() where TEntity : AbstractEntity;

        /// <summary> Поиск сущности (может не найти) </summary>
        [CanBeNull]
        TEntity Find<TEntity>(object[] keyValues) where TEntity : AbstractEntity;
    }
}