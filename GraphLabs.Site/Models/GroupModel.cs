using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.EF.Extensions;
using GraphLabs.DomainModel.EF.Services;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace GraphLabs.Site.Models
{
    public class GroupModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsOpen { get; set; }

        public ICollection<Student> Students { get; set; }

        public int FirstYear { get; set; }

        public int Number { get; set; }

        /// <summary> Конструктор по умолчанию </summary>
        public GroupModel()
        {
        }

        /// <summary> Конструктор, принимающий группу и сервис даты </summary>
        public GroupModel(Group group, ISystemDateService dateService)
        {
            Contract.Requires(group != null);

            Id = group.Id;
            IsOpen = group.IsOpen;
            Students = group.Students;
            FirstYear = group.FirstYear;
            Number = group.Number;
            Name = group.GetName(dateService);
        }
    }
}