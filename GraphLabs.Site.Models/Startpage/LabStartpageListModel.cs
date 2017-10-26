using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Startpage
{
    /// <summary> Модель расписания (списка) </summary>
    public class LabStartpageListModel : ListModelBase<LabStartpageModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<LabStartpageModel, AbstractLabSchedule> _modelLoader;

        public LabStartpageListModel(
            IEntityQuery query,
            IEntityBasedModelLoader<LabStartpageModel, AbstractLabSchedule> modelLoader)
        {
            _query = query;
            _modelLoader = modelLoader;
        }

        protected override LabStartpageModel[] LoadItems()
        {
            return _query
                .OfEntities<AbstractLabSchedule>()
                .OrderBy(m => m.DateFrom)
                .ThenBy(m => m.DateTill)
               /* .Where()*/
                .ToArray()
                .Select(_modelLoader.Load)
                .ToArray();
        }
    }
}