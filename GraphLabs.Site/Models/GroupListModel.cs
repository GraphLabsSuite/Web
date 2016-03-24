using System;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;

namespace GraphLabs.Site.Models
{
    public class GroupListModel
    {
        private readonly IGraphLabsContext _context;
        private readonly ISystemDateService _dataService;

        public GroupListModel(IGraphLabsContext context, ISystemDateService dateService)
        {
            _context = context;
            _dataService = dateService;
        }

        public GroupModel[] GetGroupList()
        {
            Contract.Ensures(Contract.Result<GroupModel[]>() != null);
            return _context.Query<Group>().ToArray().Select(g => new GroupModel(g, _dataService)).ToArray();
        }

        public GroupModel GetById(long id)
        {
            Contract.Requires<ArgumentException>(id > 0);
            Contract.Ensures(Contract.Result<GroupModel>() != null);

            return _context.Query<Group>().ToArray().Select(n => new GroupModel(n, _dataService)).Single(n => n.Id == id);
        }

        public bool CreateNew(GroupModel group)
        {
            if (group.Id == 0)
            {
                var newGroup = _context.Create<Group>();
                newGroup.FirstYear = group.FirstYear;
                newGroup.IsOpen = group.IsOpen;
                newGroup.Number = group.Number;
                newGroup.Students = group.Students;

                return true;
            }

            return false;
        }

        public bool Edit(GroupModel group)
        {
            var gr = _context.Query<Group>().Single(g => g.Id == group.Id);
            gr.FirstYear = group.FirstYear;
            gr.IsOpen = group.IsOpen;
            gr.Number = group.Number;
            gr.Students = group.Students;

            return true;
        }
    }
}