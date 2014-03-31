using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с лабораторными работами </summary>
    [ContractClass(typeof(LabRepositoryContracts))]
    public interface ILabRepository : IDisposable
    {
        /// <summary> Получить лабораторные работы </summary>
        [NotNull]
        LabWork[] GetLabWorks();

        /// <summary> Получить лабораторную работу по id </summary>
        [NotNull]
        LabWork GetLabWorkById(long id);

        /// <summary> Получить варианты лабораторной работы по id лабораторной работы </summary>
        [NotNull]
        LabVariant[] GetLabVariantsByLabWorkId(long id);

        /// <summary> Получить вариант лабораторной работы по id </summary>
        [NotNull]
        LabVariant GetLabVariantById(long id);
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
        public LabWork GetLabWorkById(long id)
        {
            Contract.Requires(id > 0);
            Contract.Ensures(Contract.Result<LabWork>() != null);

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
        public LabVariant GetLabVariantById(long id)
        {
            Contract.Requires(id > 0);
            Contract.Ensures(Contract.Result<LabVariant>() != null);

            return default(LabVariant);
        }
    }
}
