using System.Linq;
using System.Data.Entity;
using System;

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

            return Context.Groups.Single(g => g.Id == id);
        }

        /// <summary> Попробовать сохранить новую группу </summary>
        public bool TrySaveGroup(Group group)
        {
            CheckNotDisposed();

            try
            {
                Context.Groups.Add(group);
                Context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary> Попробовать обновить группу </summary>
        public bool TryModifyGroup(long id, int number, int year, bool isOpen)
        {
            Group group = GetGroupById(id);
            group.Number = number;
            group.FirstYear = year;
            group.IsOpen = isOpen;
            try
            {
                Context.Entry(group).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
