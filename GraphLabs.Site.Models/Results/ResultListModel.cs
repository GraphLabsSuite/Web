using System;
using System.Collections.Generic;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System.Linq;

namespace GraphLabs.Site.Models.Results
{
    public class ResultListModel : ListModelBase<ResultModel>, 
        IFilterableByDate<ResultListModel, ResultModel>, 
        IFilterableByUser<ResultListModel, ResultModel>
    {
        private readonly IEntityBasedModelLoader<ResultModel, Result> _resultLoader;
        private readonly IEntityQuery _query;
        public ResultListModel(IEntityBasedModelLoader<ResultModel, Result> resultLoader,
            IEntityQuery query)
        {
            _resultLoader = resultLoader;
            _query = query;
        }
        protected override ResultModel[] LoadItems()
        {
            return _query
                .OfEntities<Result>()
                .Where(r => r.Student.Email == _email)
                .Where(r => (_from == null || r.StartDateTime >= _from) && (_till == null || r.StartDateTime <= _till))
                .ToArray()
                .Select(r => _resultLoader.Load(r))
                .ToArray();
        }

        private DateTime? _from;
        private DateTime? _till;
        private string _email;
        public ResultListModel FilterByDate(DateTime? from, DateTime? till)
        {
            _from = from;
            _till = till;
            InvalidateItems();
            return this;
        }
        

        public ResultListModel FilterByUser(string email)
        {
            _email = email;
            InvalidateItems();
            return this;
        }
    }
}
