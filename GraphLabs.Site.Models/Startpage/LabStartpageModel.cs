using System;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Startpage
{
    /// <summary> Модель <see cref="AbstractLabSchedule"/> </summary>
    public class LabStartpageModel : IEntityBasedModel<AbstractLabSchedule>
    {
        public long Id { get; set; }
        public string LabName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTill { get; set; }
        public LabExecutionMode Mode { get; set; }
        public string Doer { get; set; }
    }
}
