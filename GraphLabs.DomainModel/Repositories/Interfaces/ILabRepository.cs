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

        /// <summary> Найти лабораторную работу по id </summary>
        [CanBeNull]
        LabWork FindLabWorkById(long id);

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

        /// <summary> Найти вариант лабораторной работы по id </summary>
        [CanBeNull]
        LabVariant FindLabVariantById(long id);

        /// <summary> Получить задания лабораторной работы по какому-либо варианту лабораторной работы </summary>
        [CanBeNull]
        Task[] FindEntryTasksByLabVarId(long labVarId);

        /// <summary> Получить задания варианта лабораторной работы </summary>
        [CanBeNull]
        Task[] FindTasksByLabVarId(long labVarId);

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

        /// <summary> Репозиторий с лабораторными работами </summary>
        public LabWork[] GetLabWorks()
        {
            Contract.Ensures(Contract.Result<LabWork[]>() != null);

            return new LabWork[0];
        }

        /// <summary> Репозиторий с лабораторными работами </summary>
        public LabWork[] GetDemoLabs(DateTime currentDate)
        {
            Contract.Requires(currentDate != null);
            Contract.Ensures(Contract.Result<LabWork[]>() != null);

            return new LabWork[0];
        }

        /// <summary> Репозиторий с лабораторными работами </summary>
        public LabWork FindLabWorkById(long id)
        {
            Contract.Requires(id > 0);

            return default(LabWork);
        }

        /// <summary> Репозиторий с лабораторными работами </summary>
        public LabVariant[] GetLabVariantsByLabWorkId(long id)
        {
            Contract.Requires(id > 0);
            Contract.Ensures(Contract.Result<LabVariant[]>() != null);

            return new LabVariant[0];
        }

        /// <summary> Репозиторий с лабораторными работами </summary>
        public LabVariant[] GetDemoLabVariantsByLabWorkId(long labId)
        {
            Contract.Requires(labId > 0);
            Contract.Ensures(Contract.Result<LabVariant[]>() != null);

            return new LabVariant[0];
        }

        /// <summary> Репозиторий с лабораторными работами </summary>
        public LabVariant[] GetCompleteDemoLabVariantsByLabWorkId(long labId)
        {
            Contract.Requires(labId > 0);
            Contract.Ensures(Contract.Result<LabVariant[]>() != null);

            return new LabVariant[0];
        }

        /// <summary> Репозиторий с лабораторными работами </summary>
        public LabVariant FindLabVariantById(long id)
        {
            Contract.Requires(id > 0);

            return default(LabVariant);
        }

        public Task[] FindEntryTasksByLabVarId(long labVarId)
        {
            Contract.Requires(labVarId > 0);

            return new Task[0];
        }

        public Task[] FindTasksByLabVarId(long labVarId)
        {
            Contract.Requires(labVarId > 0);

            return new Task[0];
        }

        public TaskVariant[] GetTaskVariantsByLabVarId(long labVarId)
        {
            Contract.Requires(labVarId > 0);
            Contract.Ensures(Contract.Result<TaskVariant[]>() != null);

            return new TaskVariant[0];
        }
    }
}
