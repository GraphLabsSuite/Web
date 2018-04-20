using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
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
        private static readonly ConcurrentDictionary<Type, MethodInfo> _methodInfos = new ConcurrentDictionary<Type, MethodInfo>();
        
        protected Expression<Func<TEntity, bool>> FiExpression { get; private set; }

        public abstract ActionResult Index(string message);
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.ActionName.Equals(nameof(Index)))//может быть это ограничение лишнее
            {
                FilterParams filterParams = new FilterParams(Request.QueryString);
                Type type = typeof(TModel);
                if (!_methodInfos.ContainsKey(type))
                {
                    _methodInfos.TryAdd(type, type.GetMethod(nameof(AbstractFilterableModel<Object>.CreateFilter)));
                }
                
                FiExpression = (Expression<Func<TEntity, bool>>) _methodInfos[type].Invoke(null, new[] {filterParams});
            }
        }
    }
}