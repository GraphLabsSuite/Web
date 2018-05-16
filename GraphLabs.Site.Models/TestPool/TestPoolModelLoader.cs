using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TestPoolEntry;

namespace GraphLabs.Site.Models.TestPool
{
    /// <summary> Загрузчик моделей тестпулов </summary>
    internal sealed class TestPoolModelLoader : AbstractModelLoader<TestPoolModel, DomainModel.TestPool>
    {
        private readonly IEntityBasedModelLoader<TestPoolEntryModel, DomainModel.TestPoolEntry> _entryLoader;

        /// <summary> Загрузчик моделей тестпулов </summary>
        public TestPoolModelLoader(
            IEntityQuery query,
            IEntityBasedModelLoader<TestPoolEntryModel, DomainModel.TestPoolEntry> entryLoader) : base(query)
        {
            _entryLoader = entryLoader;
        }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override TestPoolModel Load(DomainModel.TestPool testPool)
        {
            Contract.Requires(testPool != null);
            var array = testPool.TestPoolEntries.Select(_entryLoader.Load).ToArray();

            var model = new TestPoolModel
            {
                Id = testPool.Id,
                Name = testPool.Name,
                TestPoolEntries = array
            };


            return model;
        }
    }
}
