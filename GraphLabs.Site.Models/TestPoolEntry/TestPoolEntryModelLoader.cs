using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Groups;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.TestPool;
using Microsoft.Practices.ObjectBuilder2;

namespace GraphLabs.Site.Models.TestPoolEntry
{
    /// <summary> Загрузчик моделей тестпулов </summary>
    internal sealed class TestPoolEntryModelLoader : AbstractModelLoader<TestPoolEntryModel, DomainModel.TestPoolEntry>
    {

        private readonly IEntityBasedModelLoader<TestPoolModel, DomainModel.TestPool> _testPoolModelLoader;
        private readonly IEntityBasedModelLoader<TestPoolEntryModel, DomainModel.TestPoolEntry> _testPoolEntryModelLoader;

        /// <summary> Загрузчик моделей тестпулов </summary>
        public TestPoolEntryModelLoader(
            IEntityQuery query,
            IEntityBasedModelLoader<TestPoolModel, DomainModel.TestPool> testPoolModelLoader,
            IEntityBasedModelLoader<TestPoolEntryModel, DomainModel.TestPoolEntry> testPoolEntryModelLoader
            ) : base(query)
        {
            _testPoolModelLoader = testPoolModelLoader;
            _testPoolEntryModelLoader = testPoolEntryModelLoader;
        }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override TestPoolEntryModel Load(DomainModel.TestPoolEntry testPoolEntry)
        {
            Contract.Requires(testPoolEntry != null);

            var model = new TestPoolEntryModel()
            {
                Id = testPoolEntry.Id,
                Score = testPoolEntry.Score,
                ScoringStrategy = testPoolEntry.ScoringStrategy,
                TestQuestion = testPoolEntry.TestQuestion,
                TestPool = testPoolEntry.TestPool
            };

            return model;
        }
    }
}
