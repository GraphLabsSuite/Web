using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с тестпулами </summary>
    [ContractClass(typeof(TestPoolRepositoryContracts))]
    [Obsolete("Использовать глобальный контекст IGraphLabsContext")]
    public interface ITestPoolRepository : IDisposable
    {
        /// <summary> Получить тестпул по id </summary>
        [NotNull]
        TestPool GetTestPoolById(long id);
    }

    /// <summary> Репозиторий с тестпулами </summary>
    [ContractClassFor(typeof(ITestPoolRepository))]
    internal abstract class TestPoolRepositoryContracts : ITestPoolRepository
    {
        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
        }

        public TestPool GetTestPoolById(long id)
        {
            Contract.Requires(id > 0);
            return default(TestPool);
        }

    }
}