using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabVariants
{
    /// <summary> Модель списка вариантов лабораторных работ </summary>
    public sealed class LabVariantListModel : ListModelBase<LabVariantModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<LabVariantModel, LabVariant> _modelLoader;

        /// <summary> Модель списка вариантов лабораторных работ </summary>
        public LabVariantListModel(IEntityQuery query, IEntityBasedModelLoader<LabVariantModel, LabVariant> modelLoader)
        {
            _query = query;
            _modelLoader = modelLoader;
        }

        /// <summary> Загружает варианты лабораторных работ </summary>
        protected override LabVariantModel[] LoadItems()
        {
            return _query.OfEntities<LabVariant>()
                .ToArray()
                .Select(_modelLoader.Load)
                .ToArray();
        }
    }
}
