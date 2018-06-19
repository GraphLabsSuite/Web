using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TestPoolEntry;

namespace GraphLabs.Site.Models.TestPool
{
    internal sealed class TestPoolModelLoader : AbstractTestPoolModelLoader<TestPoolModel>
    {
        public TestPoolModelLoader(IEntityQuery query, IEntityBasedModelLoader<TestPoolEntryModel, DomainModel.TestPoolEntry> entryLoader) : base(query, entryLoader)
        {
        }
    }
    internal sealed class TestPoolEditModelLoader : AbstractTestPoolModelLoader<TestPoolEditModel>
    {
        private readonly IEntityBasedModelLoader<SubCategoryModel, SubCategory> _subCategoryLoader;
        public TestPoolEditModelLoader(IEntityQuery query, 
            IEntityBasedModelLoader<TestPoolEntryModel, DomainModel.TestPoolEntry> entryLoader,
            IEntityBasedModelLoader<SubCategoryModel, SubCategory> subCategoryLoader) : base(query, entryLoader)

        {
            _subCategoryLoader = subCategoryLoader;
        }
        public override TestPoolEditModel Load(DomainModel.TestPool testPool)
        {
            var model = base.Load(testPool);
            model.AllSubCategories = _query.OfEntities<SubCategory>().ToArray().Select(_subCategoryLoader.Load).ToArray();
            return model;
        }
    }
    /// <summary> Загрузчик моделей тестпулов </summary>
    internal abstract class AbstractTestPoolModelLoader<TModel> : AbstractModelLoader<TModel, DomainModel.TestPool>
        where TModel:TestPoolModel, new()
    {
        private readonly IEntityBasedModelLoader<TestPoolEntryModel, DomainModel.TestPoolEntry> _entryLoader;

        /// <summary> Загрузчик моделей тестпулов </summary>
        protected AbstractTestPoolModelLoader(
            IEntityQuery query,
            IEntityBasedModelLoader<TestPoolEntryModel, DomainModel.TestPoolEntry> entryLoader) : base(query)
        {
            _entryLoader = entryLoader;
        }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override TModel Load(DomainModel.TestPool testPool)
        {
            Contract.Requires(testPool != null);
            var array = testPool.TestPoolEntries.Select(_entryLoader.Load).ToArray();

            var model = new TModel
            {
                Id = testPool.Id,
                Name = testPool.Name,
                TestPoolEntries = array
            };


            return model;
        }
    }
}
