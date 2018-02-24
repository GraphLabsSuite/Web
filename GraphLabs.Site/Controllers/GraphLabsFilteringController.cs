using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using GraphLabs.DomainModel.Infrastructure;
using GraphLabs.Site.Core.Filters;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Controllers
{
    public abstract class GraphLabsFilteringController<TModel, TEntity> : GraphLabsController
        where TEntity : AbstractEntity
        where TModel : AbstractFilterableModel<TEntity>, IEntityBasedModel<TEntity>
    {
        protected Expression<Func<TEntity, bool>> _fiExpression;
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.ActionName.Equals("Index"))//может быть это ограничение лишнее
            {
                FilterParams _filterParams = new FilterParams(Request.QueryString);
                _fiExpression = (Expression<Func<TEntity, bool>>) typeof(TModel).GetMethod("CreateFilter").Invoke(null, new[] {_filterParams});
            }
        }
    }
}