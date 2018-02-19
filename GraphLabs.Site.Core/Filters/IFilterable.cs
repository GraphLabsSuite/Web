using System;
using System.Linq.Expressions;
using GraphLabs.DomainModel.Infrastructure;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Core.Filters
{
    public interface IFilterable<TEntity, TModel> 
        where TEntity : AbstractEntity
        where TModel : IFilterableModel<TEntity>
    {
        IListModel<TModel> filter(Expression<Func<TEntity, bool>> filter);
    }

}