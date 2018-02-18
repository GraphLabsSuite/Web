using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GraphLabs.DomainModel;
using GraphLabs.Site.Core;
using GraphLabs.Site.Core.Filters;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Groups
{
    /// <summary> Модель списка групп </summary>
    public sealed class GroupListModel : ListModelBase<GroupModel>,
            IFilterableByName<GroupListModel, GroupModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<GroupModel, Group> _modelLoader;
        private string _name = "";

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
                .Where(m => _name == "" || _name.Equals(m.Name))
                .ToArray()
                .Select(_modelLoader.Load)
                .ToArray();
        }

        public GroupListModel FilterByName(String name)
        {
            _name = name ?? "";
            return this;
        }

        public string FilterableByNameText()
        {
            return "Номер группы";
        }
    }
}