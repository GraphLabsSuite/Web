using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.TestPoolEntry
{
    /// <summary>
    /// Модель для сохранения вопроса в тестпуле
    /// </summary>
    public class SaveTestPoolEntryModel : IEntityBasedModel<DomainModel.TestPoolEntry>
    {
        public long Id { get; set; }

        public long SubCategoryId { get; set; }

        public long TestPool { get; set; }
    }
}