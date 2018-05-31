using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace GraphLabs.DomainModel.Infrastructure
{
    /// <summary> Интерфейс, описывающий изменение сущности </summary>
    //[ContractClass(typeof(EntityChangeContracts))]
    public interface IEntityChange
    {
        /// <summary> Изменилось ли свойство? </summary>
        bool PropertyChanged(string propertyName);

        /// <summary> Исходные значения свойств </summary>
        IReadOnlyDictionary<string, object> OriginalValues { get; }

        /// <summary> Текущие значения свойств </summary>
        IReadOnlyDictionary<string, object> CurrentValues { get; }
    }
}

    /// <summary> Контракты для <see cref="IEntityChange"/> </summary>
//    [ContractClassFor(typeof(IEntityChange))]
//    abstract class EntityChangeContracts : IEntityChange
//    {
//        /// <summary> Изменилось ли свойство? </summary>
//        public bool PropertyChanged(string propertyName)
//        {
//            Contract.Requires<ArgumentException>(string.IsNullOrWhiteSpace(propertyName));
//            Guard.IsNotNullOrWhiteSpace(propertyName);
//            return default(bool);
//        }

//        /// <summary> Исходные значения свойств </summary>
//        public IReadOnlyDictionary<string, object> OriginalValues
//        {
//            get
//            {
//                Contract.Ensures(Contract.Result<IReadOnlyDictionary<string, object>>() != null);//переписать сразу у наследников
//                return default(IReadOnlyDictionary<string, object>);
//            }
//        }

//        /// <summary> Текущие значения свойств </summary>
//        public IReadOnlyDictionary<string, object> CurrentValues
//        {
//            get
//            {
//                Contract.Ensures(Contract.Result<IReadOnlyDictionary<string, object>>() != null);// у наследников
//                return default(IReadOnlyDictionary<string, object>);
//            }
//        }
//    }
//}