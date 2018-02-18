using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Schedule
{
    /// <summary> Модель расписания (списка) </summary>
    public class LabScheduleListModel : ListModelBase<LabScheduleModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<LabScheduleModel, AbstractLabSchedule> _modelLoader;

        public LabScheduleListModel(
            IEntityQuery query,
            IEntityBasedModelLoader<LabScheduleModel, AbstractLabSchedule> modelLoader)
        {
            _query = query;
            _modelLoader = modelLoader;
        }

        protected override LabScheduleModel[] LoadItems()
        {
            return _query
                .OfEntities<AbstractLabSchedule>()
                .OrderBy(m => m.DateFrom)
                .ThenBy(m => m.DateTill)
                .ToArray()
                .Select(_modelLoader.Load)
                .ToArray();
        }
    }
}