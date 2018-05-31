using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Infrastructure
{
    /// <summary> Отслеживаемые сущности </summary>
  //  [ContractClass(typeof(TrackableEntityContracts))]
    public interface ITrackableEntity
    {
        /// <summary> Перед сохранением новой сущности в базу </summary>
        void OnInsert();

        /// <summary> Перед сохранением изменённой сущности в базу </summary>
        void OnChange([NotNull]IEntityChange change);

        /// <summary> Валидация </summary>
        IEnumerable<EntityValidationError> OnValidating();
    }

    /// <summary> Контракты для <see cref="ITrackableEntity"/> </summary>
    //[ContractClassFor(typeof(ITrackableEntity))]
    //abstract class TrackableEntityContracts : ITrackableEntity
    //{
    //    /// <summary> Перед сохранением новой сущности в базу </summary>
    //    public void OnInsert()
    //    {
    //    }

    //    /// <summary> Перед сохранением изменённой сущности в базу </summary>
    //    public void OnChange(IEntityChange change)
    //    {
    //        Contract.Requires<ArgumentNullException>(change != null);
    //    }

    //    /// <summary> Валидация </summary>
    //    public IEnumerable<EntityValidationError> OnValidating()
    //    {
    //        Contract.Ensures(Contract.Result<IEnumerable<EntityValidationError>>() != null);
    //        return default(IEnumerable<EntityValidationError>);
    //    }
    //}
}