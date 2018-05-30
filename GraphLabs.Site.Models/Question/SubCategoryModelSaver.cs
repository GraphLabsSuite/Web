using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GraphLabs.Site.Models.Question
{
    

    /// <summary> Сервис сохранения категорий </summary>
    sealed class SubCategoryModelSaver : AbstractModelSaver<SubCategoryModel, SubCategory>
    {
        /// <summary> Сервис сохранения категорий </summary>
        public SubCategoryModelSaver(IOperationContextFactory<IGraphLabsContext> operationContextFactory)
            : base(operationContextFactory)
        {
        }

        protected override Action<SubCategory> GetEntityInitializer(SubCategoryModel model, IEntityQuery query)
        {
            return g =>
            {
                g.Id = model.Id;
                g.Name = model.Name;
                g.Category.Id = model.CategoryId;
            };
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ </remarks>
        protected override bool ExistsInDatabase(SubCategoryModel model)
        {
            return model.Id != 0;
        }

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected override object[] GetEntityKey(SubCategoryModel model)
        {
            return new object[] { model.Id };
        }
    }
}