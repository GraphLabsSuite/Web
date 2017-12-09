using System.Diagnostics.Contracts;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Groups
{
    /// <summary> Загрузчик моделей групп </summary>
    sealed class GroupModelLoader : AbstractModelLoader<GroupModel, Group>
    {
        private readonly ISystemDateService _dateService;

        /// <summary> Загрузчик моделей групп </summary>
        public GroupModelLoader(IEntityQuery query, ISystemDateService dateService)
            : base(query)
        {
            _dateService = dateService;
        }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override GroupModel Load(Group group)
        {
            Contract.Requires(group != null);

            var model = new GroupModel
            {
                Id = group.Id,
                IsOpen = group.IsOpen,
                Students = group.Students,
                FirstYear = group.FirstYear,
                Name = group.Name

            };

            return model;
        }
    }
}