using System;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Schedule
{
    /// <summary> Модель <see cref="AbstractLabSchedule"/> </summary>
    public class LabScheduleModel : IEntityBasedModel<AbstractLabSchedule>
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTill { get; set; }
        public LabExecutionMode Mode { get; set; }
        public string Doer { get; set; }
    }
}
