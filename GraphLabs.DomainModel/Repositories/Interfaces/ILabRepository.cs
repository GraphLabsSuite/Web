using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с лабораторными работами </summary>
    [ContractClass(typeof(LabRepositoryContracts))]
    public interface ILabRepository : IDisposable
    {
        // ReSharper disable ReturnTypeCanBeEnumerable.Global

        #region Получение массивов лабораторных работ

        /// <summary> Получить лабораторные работы </summary>
        [NotNull]
        LabWork[] GetLabWorks();

        /// <summary> Получить лабораторные работы, доступные в данный момент в ознакомительном режиме </summary>
        [NotNull]
        LabWork[] GetDemoLabs(DateTime currentDate);

        #endregion

        #region Получение массива вариантов лабораторной работы

        /// <summary> Получить варианты лабораторной работы по id лабораторной работы </summary>
        [NotNull]
        LabVariant[] GetLabVariantsByLabWorkId(long id);

        /// <summary> Получить ознакомительные варианты лабораторной работы по id лабораторной работы </summary>
        [NotNull]
        LabVariant[] GetDemoLabVariantsByLabWorkId(long id);

        /// <summary> Получить готовые ознакомительные варианты лабораторной работы по id лабораторной работы </summary>
        [NotNull]
        LabVariant[] GetCompleteDemoLabVariantsByLabWorkId(long labId);

        #endregion

        #region Проверки

        /// <summary> Проверить существование лабораторной работы </summary>
        [NotNull]
        bool CheckLabWorkExist(long id);

        /// <summary> Проверить существование варианта лабораторной работы </summary>
        [NotNull]
        bool CheckLabVariantExist(long id);

        /// <summary> Проверить принадлежность варианта л.р. лабораторной работе </summary>
        [NotNull]
        bool CheckLabVariantBelongLabWork(long labId, long labVarId);

        /// <summary> Проверка соответствия варианта лабораторной работы содержанию работы </summary>
        [NotNull]
        bool VerifyCompleteVariant(long variantId);

        #endregion

        /// <summary> Получить лабораторную работу по id </summary>
        [CanBeNull]
        LabWork GetLabWorkById(long id);

        /// <summary> Найти вариант лабораторной работы по id </summary>
        [CanBeNull]
        LabVariant FindLabVariantById(long id);

        /// <summary> Получить варианты заданий с заданиями варианта лабораторной работы </summary>
        [NotNull]
        TaskVariant[] GetTaskVariantsByLabVarId(long labVarId);
    }

    /// <summary> Репозиторий с лаораторными работами - контракты </summary>
    [ContractClassFor(typeof(ILabRepository))]
    internal abstract class LabRepositoryContracts : ILabRepository
    {
        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
        }

        #region Получение массива лабораторных работ

        public LabWork[] GetLabWorks()
        {
            Contract.Ensures(Contract.Result<LabWork[]>() != null);

            return new LabWork[0];
        }

        public LabWork[] GetDemoLabs(DateTime currentDate)
        {
            Contract.Requires(currentDate != null);
            Contract.Ensures(Contract.Result<LabWork[]>() != null);

            return new LabWork[0];
        }

        #endregion

        #region Получение массива вариантов лабораторной работы

        public LabVariant[] GetLabVariantsByLabWorkId(long id)
        {
            Contract.Requires(id > 0);
            Contract.Ensures(Contract.Result<LabVariant[]>() != null);

            return new LabVariant[0];
        }

        public LabVariant[] GetDemoLabVariantsByLabWorkId(long labId)
        {
            Contract.Requires(labId > 0);
            Contract.Ensures(Contract.Result<LabVariant[]>() != null);

            return new LabVariant[0];
        }

        public LabVariant[] GetCompleteDemoLabVariantsByLabWorkId(long labId)
        {
            Contract.Requires(labId > 0);
            Contract.Ensures(Contract.Result<LabVariant[]>() != null);

            return new LabVariant[0];
        }

        #endregion

        #region Проверки

        public bool CheckLabWorkExist(long id)
        {
            Contract.Requires(id > 0);

            return false;
        }

        public bool CheckLabVariantExist(long id)
        {
            Contract.Requires(id > 0);

            return false;
        }

        public bool CheckLabVariantBelongLabWork(long labId, long labVarId)
        {
            Contract.Requires(labId > 0);
            Contract.Requires(labVarId > 0);

            return false;
        }

        public bool VerifyCompleteVariant(long variantId)
        {
            Contract.Requires(variantId > 0);

            return false;
        }

        #endregion

        public LabWork GetLabWorkById(long id)
        {
            Contract.Requires(id > 0);
            Contract.Ensures(Contract.Result<LabWork>() != null);

            return default(LabWork);
        }

        public LabVariant FindLabVariantById(long id)
        {
            Contract.Requires(id > 0);

            return default(LabVariant);
        }

        public TaskVariant[] GetTaskVariantsByLabVarId(long labVarId)
        {
            Contract.Requires(labVarId > 0);
            Contract.Ensures(Contract.Result<TaskVariant[]>() != null);

            return new TaskVariant[0];
        }
    }
}
