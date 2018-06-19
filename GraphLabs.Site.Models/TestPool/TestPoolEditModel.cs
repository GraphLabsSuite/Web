using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLabs.Site.Models.TestPool
{

    public class TestPoolEditModel : TestPoolModel
    {
        public SubCategoryModel[] AllSubCategories { get; set; }
    }
}
