using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GraphLabs.Site.Models.Question
{
    public class SubCategoryModel : IEntityBasedModel<SubCategory>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CategoryId { get; set; }
    }

}
