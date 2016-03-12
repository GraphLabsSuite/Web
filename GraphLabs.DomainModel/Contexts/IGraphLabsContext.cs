using System;
using System.Linq;
using GraphLabs.DomainModel.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Контекст GraphLabs </summary>
    //TODO Пока только интерфейс, реализацию сделать не успел
    public interface IGraphLabsContext
    {
        /// <summary> Запрос сущностей </summary>
        [NotNull]
        IQueryable<TEntity> Query<TEntity>() where TEntity : AbstractEntity;

        /// <summary> Поиск сущности (может не найти) </summary>
        [CanBeNull]
        TEntity Find<TEntity>(params object[] keyValues) where TEntity : AbstractEntity;

        /// <summary> Создаёт новый экземпляр сущности </summary>
        TEntity Create<TEntity>() where TEntity : AbstractEntity;
    }
}