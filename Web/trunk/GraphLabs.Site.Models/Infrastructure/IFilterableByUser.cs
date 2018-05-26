namespace GraphLabs.Site.Models.Infrastructure
{
    public interface IFilterableByUser<TListModel, TModel>
        where TListModel : IListModel<TModel>
    {
        TListModel FilterByUser(string user);
    }
}