using System.Linq;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с группами </summary>
    internal class GroupRepository : RepositoryBase, IGroupRepository
    {
        /// <summary> Репозиторий с группами </summary>
        public GroupRepository(GraphLabsContext context)
            : base(context)
        {
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

            return Context.Groups.Single(g => g.Id == id);
        }
    }
}
