using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabWorks
{
    /// <summary> Модель списка лабораторных работ </summary>
    public sealed class LabWorkListModel : ListModelBase<LabWorkModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<LabWorkModel, DomainModel.LabWork> _modelLoader;

        /// <summary> Модель списка дабораторных работ </summary>
        public LabWorkListModel(IEntityQuery query, IEntityBasedModelLoader<LabWorkModel, DomainModel.LabWork> modelLoader)
        {
            _query = query;
            _modelLoader = modelLoader;
        }

        /// <summary> Загружает лабораторные работы </summary>
        protected override LabWorkModel[] LoadItems()
        {
            return _query.OfEntities<DomainModel.LabWork>()
                .ToArray()
                .Select(_modelLoader.Load)
                .ToArray();
        }
    }
}
