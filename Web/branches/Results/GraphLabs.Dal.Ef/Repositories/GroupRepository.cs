using System;
using System.Data.Entity;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Dal.Ef.Repositories
{
    /// <summary> Репозиторий с группами </summary>
    internal class GroupRepository : RepositoryBase, IGroupRepository
    {
        /// <summary> Репозиторий с группами </summary>
        public GroupRepository(GraphLabsContext context)
            : base(context)
        {
        }

        /// <summary> Получить все группы </summary>
        public Group[] GetAllGroups()
        {
            CheckNotDisposed();

            return Context.Groups.ToArray();
        }

        /// <summary> Получить группы, открытые для регистрации </summary>
        public Group[] GetOpenGroups()
        {
            CheckNotDisposed();

            return Context.Groups.Where(g => g.IsOpen).ToArray();
        }

        /// <summary> Получить группу по id </summary>
        public Group GetGroupById(long id)
        {
            CheckNotDisposed();

            return Context.Groups.Where(g => g.Id == id).Single();
        }
    }
}
