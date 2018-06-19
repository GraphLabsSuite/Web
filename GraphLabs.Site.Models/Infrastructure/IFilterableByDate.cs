using System;

namespace GraphLabs.Site.Models.Infrastructure
{
    public interface IFilterableByDate<TListModel, TModel>
        where TListModel : IListModel<TModel>
    {
        TListModel FilterByDate(DateTime? from, DateTime? till);
    }
}