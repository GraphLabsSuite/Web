using System;
using GraphLabs.DomainModel.Infrastructure;

namespace GraphLabs.DomainModel
{
    /// <summary> Фабрика экземпляров сущностей </summary>
    public interface IEntityFactory
    {
        /// <summary> Создать сущность </summary>
        /// <param name="initializer">Инициализатор сущности</param>
        TEntity Create<TEntity>(Action<TEntity> initializer = null) where TEntity : AbstractEntity;

        /// <summary> Удалить сущность </summary>
        void Delete<TEntity>(TEntity entity) where TEntity : AbstractEntity;
    }
}