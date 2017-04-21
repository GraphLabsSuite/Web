using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Groups;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.TestPool
{
    /// <summary>
    /// Модель списка тестпулов
    /// </summary>
    public class TestPoolListModel : ListModelBase<TestPoolModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<TestPoolModel, DomainModel.TestPool> _modelLoader;

        /// <summary> Модель списка тестпулов </summary>
        public TestPoolListModel(IEntityQuery query, IEntityBasedModelLoader<TestPoolModel, DomainModel.TestPool> modelLoader)
        {
            _query = query;
            _modelLoader = modelLoader;
        }

        /// <summary> Загружает тестпулы</summary>
        protected override TestPoolModel[] LoadItems()
        {
            return _query.OfEntities<DomainModel.TestPool>()
                .ToArray()
                .Select(_modelLoader.Load)
                .ToArray();
        }

    }
}
