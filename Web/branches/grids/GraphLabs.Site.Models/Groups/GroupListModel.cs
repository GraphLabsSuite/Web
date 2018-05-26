using System;
using System.Linq;
using System.Linq.Expressions;
using GraphLabs.DomainModel;
using GraphLabs.Site.Core.Filters;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Groups
{
    /// <summary> Модель списка групп </summary>
    public sealed class GroupListModel : ListModelBase<GroupModel>, IFilterable<Group, GroupModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<GroupModel, Group> _modelLoader;
        private Expression<Func<Group, bool>> _filter = (group => true);

        /// <summary> Модель списка групп </summary>
        public GroupListModel(IEntityQuery query, IEntityBasedModelLoader<GroupModel, Group> modelLoader)
        {
            _query = query;
            _modelLoader = modelLoader;
        }

        /// <summary> Загружает группы </summary>
        protected override GroupModel[] LoadItems()
        {
            return _query.OfEntities<Group>()
                .Where(_filter)
                .ToArray()
                .Select(_modelLoader.Load)
                .ToArray();
        }

        public IListModel<GroupModel> Filter(Expression<Func<Group, bool>> filter)
        {
            _filter = filter;
            return this;
        }
    }
}