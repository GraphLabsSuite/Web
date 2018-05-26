using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с группами </summary>
    [ContractClass(typeof(GroupRepositoryContracts))]
    [Obsolete("Использовать глобальный контекст IGraphLabsContext")]
    public interface IGroupRepository : IDisposable
    {
        // ReSharper disable ReturnTypeCanBeEnumerable.Global

        /// <summary> Получить все группы </summary>
        [NotNull]
        Group[] GetAllGroups();

        /// <summary> Получить группы, открытые для регистрации </summary>
        [NotNull]
        Group[] GetOpenGroups();

        /// <summary> Получить группу по id </summary>
        [NotNull]
        Group GetGroupById(long id);
    }

    /// <summary> Репозиторий с группами </summary>
    [ContractClassFor(typeof(IGroupRepository))]
    internal abstract class GroupRepositoryContracts : IGroupRepository
    {
        // ReSharper disable AssignNullToNotNullAttribute

        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
        }

        /// <summary> Репозиторий с группами </summary>
        public Group[] GetAllGroups()
        {
            Contract.Ensures(Contract.Result<Group[]>() != null);

            return new Group[0];
        }

        /// <summary> Репозиторий с группами </summary>
        public Group[] GetOpenGroups()
        {
            Contract.Ensures(Contract.Result<Group[]>() != null);

            return new Group[0];
        }

        /// <summary> Репозиторий с группами </summary>
        public Group GetGroupById(long id)
        {
            Contract.Requires(id > 0);
            Contract.Ensures(Contract.Result<Group>() != null);

            return default(Group);
        }
    }
}
