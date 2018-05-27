using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GraphLabs.DomainModel;
using GraphLabs.Site.Core.Filters;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.LabVariant;

namespace GraphLabs.Site.Models.Lab
{
    public class LabModel : AbstractFilterableModel<LabWork>, IEntityBasedModel<LabWork>
    {
        public long Id { get; set; }

        [StringFilter("Название лабораторной работы")]
        public string Name { get; set; }

        public ICollection<LabVariantModel> LabVariants { get; set; }

        public ICollection<Task> Tasks { get; set; }
        
        public static Expression<Func<LabWork, bool>> CreateFilter(FilterParams<LabModel> filterParams)
        {
            string name = filterParams.GetStringParam(nameof(Name));

            return l => (name == null || name.ToLower().Equals(l.Name.ToLower()));
        }
    }
}