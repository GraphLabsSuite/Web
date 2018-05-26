namespace GraphLabs.Site.Models.Infrastructure
{
    public interface IFilterableByLabName<TListModel, TModel>
        where TListModel : IListModel<TModel>
    {
        TListModel FilterByLabName(string labname);
    }
}