using System.Linq;
using System.Data.Entity;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Services;

namespace GraphLabs.Site.Logic.GroupLogic
{
    public class GroupLogic
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();
        private readonly ISystemDateService _dateService = ServiceLocator.Locator.Get<ISystemDateService>();

        public Group[] GetGroupsFromDB()
        {
            var groups = (from g in _ctx.Groups
                          select g).ToArray();
            return groups;
        }

        public void SaveGroupToDB(Group group)
        {
            _ctx.Groups.Add(group);
            _ctx.SaveChanges();
            return;
        }

        public Group GetGroupByID(long Id)
        {
            return _ctx.Groups.Find(Id);
        }

        public void ModifyGroupInDB(Group group, int newNumber, int newFirstYear, bool newIsOpen)
        {
            group.Number = newNumber;
            group.FirstYear = newFirstYear;
            group.IsOpen = newIsOpen;
            _ctx.Entry(group).State = EntityState.Modified;
            _ctx.SaveChanges();
            return;
        }

        public SystemDateService GetSystemDate()
        {
            return new SystemDateService(_ctx);
        }
    }
}
