using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Lab
{
    /// <summary> Модель списка лабораторных работ </summary>
    public class LabListModel : ListModelBase<LabModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<LabModel, LabWork> _modelLoader;

        /// <summary> Модель списка лабораторных работ </summary>
        public LabListModel(IEntityQuery query, IEntityBasedModelLoader<LabModel, LabWork> modelLoader)
        {
            _query = query;
            _modelLoader = modelLoader;
        }

        /// <summary> Загружает лабораторные работы </summary>
        protected override LabModel[] LoadItems()
        {
            return _query.OfEntities<LabWork>()
                .ToArray()
                .Select(l => _modelLoader.Load(l))
                .ToArray();
        }
    }
}