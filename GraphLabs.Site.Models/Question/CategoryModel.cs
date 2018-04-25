using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLabs.Site.Models.Question
{
    public class CategoryModel : IEntityBasedModel<Category>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
