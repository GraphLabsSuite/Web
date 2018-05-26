using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Groups
{
    /// <summary> Модель списка групп </summary>
    public sealed class GroupListModel : ListModelBase<GroupModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<GroupModel, Group> _modelLoader;

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
                .ToArray()
                .Select(_modelLoader.Load)
                .ToArray();
        }
    }
}