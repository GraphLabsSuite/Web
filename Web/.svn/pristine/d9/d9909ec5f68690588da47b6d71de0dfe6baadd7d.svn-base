using System;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Groups
{
    /// <summary> Сервис сохранения групп </summary>
    sealed class GroupModelSaver : AbstractModelSaver<GroupModel, Group>
    {
        /// <summary> Сервис сохранения групп </summary>
        public GroupModelSaver(IOperationContextFactory<IGraphLabsContext> operationContextFactory)
            : base(operationContextFactory)
        {
        }

        protected override Action<Group> GetEntityInitializer(GroupModel model, IEntityQuery query)
        {
            return g =>
            {
                g.FirstYear = model.FirstYear;
                g.Name = model.Name;
                g.IsOpen = model.IsOpen;
            };
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected override bool ExistsInDatabase(GroupModel model)
        {
            return model.Id != 0;
        }

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected override object[] GetEntityKey(GroupModel model)
        {
            return new object[] {model.Id};
        }
    }
}