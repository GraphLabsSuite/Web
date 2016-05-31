using System.Linq;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.DemoLabs
{
    /// <summary> Модель списка демонстрационных лабораторных работ </summary>
    public class DemoLabListModel : ListModelBase<DemoLabModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<DemoLabModel, DomainModel.LabWork> _modelLoader;
        private readonly ISystemDateService _dateService;

        /// <summary> Модель списка демонстрационных лабораторных работ </summary>
        public DemoLabListModel(IEntityQuery query, IEntityBasedModelLoader<DemoLabModel, DomainModel.LabWork> modelLoader, ISystemDateService dateService)
        {
            _query = query;
            _modelLoader = modelLoader;
            _dateService = dateService;
        }

        /// <summary> Загружает демонстрационные лабораторные работы </summary>
        protected override DemoLabModel[] LoadItems()
        {
            var currentTime = _dateService.Now();
            return _query.OfEntities<DomainModel.LabWork>()
                .ToArray()
                .Where(l => l.AcquaintanceFrom.HasValue && l.AcquaintanceTill.HasValue)
                .Where(l => currentTime.CompareTo(l.AcquaintanceFrom) >= 0 && 
                            currentTime.CompareTo(l.AcquaintanceTill) <= 0)
                .Select(l => _modelLoader.Load(l))
                .ToArray();
        }
    }
}       