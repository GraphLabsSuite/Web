using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GraphLabs.DomainModel;
using GraphLabs.Site.Core.Filters;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Groups
{
    public class GroupModel : AbstractFilterableModel<Group>, IEntityBasedModel<Group>
    {
        public long Id { get; set; }

        [StringFilter("Номер группы")]
        public string Name { get; set; }
        
        [StringFilter("Доступность для регистрации")]
        public bool IsOpen { get; set; }

        public ICollection<Student> Students { get; set; }

        public int FirstYear { get; set; }

        public int Number { get; set; }
        
        public static Expression<Func<Group, bool>> CreateFilter(FilterParams filterParams)
        {
            string name = filterParams.GetStringParam("Name");
            bool? isOpen = filterParams.GetBoolParam("IsOpen");
            
            return g => (name == null || name.ToLower().Equals(g.Name.ToLower()))
                         && (isOpen == null || g.IsOpen == isOpen);
        }
    }
}