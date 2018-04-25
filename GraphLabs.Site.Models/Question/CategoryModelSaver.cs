using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;
using System;

namespace GraphLabs.Site.Models.Question
{
    /// <summary> Сервис сохранения категорий </summary>
    sealed class CategoryModelSaver : AbstractModelSaver<CategoryModel, Category>
    {
        /// <summary> Сервис сохранения категорий </summary>
        public CategoryModelSaver(IOperationContextFactory<IGraphLabsContext> operationContextFactory)
            : base(operationContextFactory)
        {
        }

        protected override Action<Category> GetEntityInitializer(CategoryModel model, IEntityQuery query)
        {
            return g =>
            {
                g.Id = model.Id;
                g.Name = model.Name;
            };
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ </remarks>
        protected override bool ExistsInDatabase(CategoryModel model)
        {
            return model.Id != 0;
        }

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected override object[] GetEntityKey(CategoryModel model)
        {
            return new object[] { model.Id };
        }
    }
}