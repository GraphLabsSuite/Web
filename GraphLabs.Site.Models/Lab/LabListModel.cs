using System;
using System.Linq;
using System.Linq.Expressions;
using GraphLabs.DomainModel;
using GraphLabs.Site.Core.Filters;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Lab
{
    /// <summary> Модель списка лабораторных работ </summary>
    public class LabListModel : ListModelBase<LabModel>, IFilterable<LabWork, LabModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<LabModel, LabWork> _modelLoader;
        private Expression<Func<LabWork, bool>> _filter;

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
                .Where(_filter)
                .ToArray()
                .Select(l => _modelLoader.Load(l))
                .ToArray();
        }

        public IListModel<LabModel> Filter(Expression<Func<LabWork, bool>> filter)
        {
            _filter = filter;
            return this;
        }
    }
}