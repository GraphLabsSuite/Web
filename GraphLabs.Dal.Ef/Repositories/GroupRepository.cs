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
            var result = Context.Groups.ToArray();
            Guard.IsNotNull(result);
            return result;
        }

        /// <summary> Получить группы, открытые для регистрации </summary>
        public Group[] GetOpenGroups()
        {
            CheckNotDisposed();
            var result = Context.Groups.Where(g => g.IsOpen).ToArray();
            Guard.IsNotNull(result);
            return result;
        }

        /// <summary> Получить группу по id </summary>
        public Group GetGroupById(long id)
        {
            Guard.IsPositive(id, "id");
            CheckNotDisposed();
            var result = Context.Groups.Where(g => g.Id == id).Single();
            Guard.IsNotNull(result);
            return result;
        }
    }
}
