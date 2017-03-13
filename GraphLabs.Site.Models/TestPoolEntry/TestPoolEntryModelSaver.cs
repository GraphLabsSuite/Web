using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.TestPoolEntry;
using Microsoft.Practices.ObjectBuilder2;

namespace GraphLabs.Site.Models.TestPool
{
    internal sealed class TestPoolEntryModelSaver : AbstractModelSaver<TestPoolEntryModel, DomainModel.TestPoolEntry>
    {

        private readonly IEntityBasedModelSaver<TestPoolEntryModel, DomainModel.TestPoolEntry> _testPoolEntryModelSaver;

        public TestPoolEntryModelSaver(
            IOperationContextFactory<IGraphLabsContext> operationContextFactory,
            IEntityBasedModelSaver<TestPoolEntryModel, DomainModel.TestPoolEntry> testPoolEntryModelSaver
            ) : base(operationContextFactory)
        {
            _testPoolEntryModelSaver = testPoolEntryModelSaver;
        }

        protected override Action<DomainModel.TestPoolEntry> GetEntityInitializer(TestPoolEntryModel model, IEntityQuery query)
        {
            
            //Action<DomainModel.TestPoolEntry> m = g =>
            //{
            //    g.Id = model.Id;
            //    g.Score = model.Score;
            //    g.ScoringStrategy = model.ScoringStrategy;
            //    g.TestQuestion = model.TestQuestion;
            //    g.TestResults = model.TestResults;
            //    //g.TestPool = ((new Action<DomainModel.TestPool>(pool =>
            //    //{
            //    //    pool.Id = model.TestPool.Id;
            //    //    pool.Name = model.TestPool.Name;
            //    //    pool.LabVariants = model.TestPool.LabVariants;
            //    //})).Target as DomainModel.TestPool);
            //};

            //ICollection<DomainModel.TestPoolEntry> testPoolEntries = new Collection<DomainModel.TestPoolEntry>();
            //model.TestPool.TestPoolEntries.ForEach(a =>
            //{
            //    if (a.Id == (m.Target as DomainModel.TestPoolEntry).Id)
            //    {
            //        testPoolEntries.Add(m.Target as DomainModel.TestPoolEntry);
            //    }
            //    else
            //    {
            //        testPoolEntries.Add(_testPoolEntryModelSaver.CreateOrUpdate(a));
            //    }
            //});

            //(m.Target as DomainModel.TestPoolEntry).TestPool.TestPoolEntries = testPoolEntries;
            var m = query.Get<DomainModel.TestPoolEntry>(model.Id);

            return g =>
            {
                g.Id = model.Id;
                g.Score = model.Score;
                g.ScoringStrategy = model.ScoringStrategy;
                g.TestQuestion = model.TestQuestion;
                g.TestPool = m.TestPool;
                g.TestResults = model.TestResults;
            };
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected override bool ExistsInDatabase(TestPoolEntryModel model)
        {
            return model.Id != 0;
        }

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected override object[] GetEntityKey(TestPoolEntryModel model)
        {
            return new object[] { model.Id };
        }
    }
}
