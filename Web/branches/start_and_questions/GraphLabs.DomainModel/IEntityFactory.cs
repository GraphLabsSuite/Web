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

        /// <summary> Создать сущность </summary>
        /// <remarks> Это то же самое что с TEntity, только без строгой типизации на этапе компиляции </remarks>
        /// <param name="initializer">Инициализатор сущности</param>
        /// <param name="typeOfEntity">Тип создаваемой сущности</param>
        object Create(Type typeOfEntity, Action<object> initializer = null);

        /// <summary> Удалить сущность </summary>
        void Delete<TEntity>(TEntity entity) where TEntity : AbstractEntity;
    }
}