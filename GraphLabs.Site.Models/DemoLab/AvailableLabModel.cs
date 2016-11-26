using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.DemoLab
{
    /// <summary> Абстрактная модель ЛР, доступной к выполнению студентом </summary>
    public abstract class AvailableLabModel : IEntityBasedModel<AbstractLabSchedule>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}