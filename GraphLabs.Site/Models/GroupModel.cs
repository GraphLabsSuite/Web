using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GraphLabs.DomainModel;
using System.Diagnostics.Contracts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Services;
using GraphLabs.Site.Logic.GroupLogic;

namespace GraphLabs.Site.Models
{
    public class GroupModel
    {
        private GroupLogic logic = new GroupLogic();

        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsOpen { get; set; }

        public ICollection<Student> Students { get; set; }

        public int FirstYear { get; set; }

        public int Number { get; set; }

        public GroupModel()
        {
        }

        public GroupModel(Group group)
        {
            Contract.Requires(group != null);

            Id = group.Id;
            IsOpen = group.IsOpen;
            Students = group.Students;
            FirstYear = group.FirstYear;
            Number = group.Number;
            Name = group.GetName(logic.GetSystemDate());
        }
    }
}