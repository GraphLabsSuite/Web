using System;
using GraphLabs.Site.Models.Infrastructure;



namespace GraphLabs.Site.Core.Filters
{
    
    //convention :  название должно быть в духе ***By***  - привет Post
    public interface IFilterableByName<TListModel, TModel>
        where TListModel : IListModel<TModel>
    {
        TListModel FilterByName(String name);
        //convention : ***Text  - отображение на форме - воообще любое назвнаие метода, но кончающееся на Text
        string FilterableByNameText();
    }
}