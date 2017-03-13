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
using GraphLabs.Site.Models.TestPoolEntry;
using Microsoft.Practices.ObjectBuilder2;

namespace GraphLabs.Site.Models.TestPool
{
    /// <summary> Загрузчик моделей тестпулов </summary>
    internal sealed class TestPoolModelLoader : AbstractModelLoader<TestPoolModel, DomainModel.TestPool>
    {

        /// <summary> Загрузчик моделей тестпулов </summary>
        public TestPoolModelLoader(
            IEntityQuery query) : base(query)
        {
        }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override TestPoolModel Load(DomainModel.TestPool testPool)
        {
            Contract.Requires(testPool != null);
            var array = new Collection<TestPoolEntryModel>();
            testPool.TestPoolEntries.ForEach(a =>
            {
                array.Add(new TestPoolEntryModel
                {
                    Id = a.Id,
                    Score =  a.Score,
                    ScoringStrategy = a.ScoringStrategy,
                    TestQuestion = a.TestQuestion,
                    TestResults = a.TestResults
                });
            });

            var model = new TestPoolModel()
            {
                Id = testPool.Id,
                Name = testPool.Name,
                TestPoolEntries = array
            };


            return model;
        }
    }
}
