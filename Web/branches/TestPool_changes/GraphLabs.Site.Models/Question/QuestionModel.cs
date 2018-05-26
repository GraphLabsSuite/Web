using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLabs.Site.Models.Question
{
    public class QuestionModel : IEntityBasedModel<TestQuestion>
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual Category Category { get; set; }
    }
}
