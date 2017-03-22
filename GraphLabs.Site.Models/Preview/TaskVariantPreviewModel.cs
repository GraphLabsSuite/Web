using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Preview
{
    public class TaskVariantPreviewModel : IEntityBasedModel<TaskVariant>
    {
        /// <summary> Id задания </summary>
        public long TaskId { get; set; }

        /// <summary> Строка инициализации </summary>
        public string InitParams { get; set; }
    }
}
