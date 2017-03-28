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
    internal sealed class SaveTestPoolEntryModelSaver : AbstractModelSaver<SaveTestPoolEntryModel, DomainModel.TestPoolEntry>
    {
        public SaveTestPoolEntryModelSaver(
            IOperationContextFactory<IGraphLabsContext> operationContextFactory
            ) : base(operationContextFactory)
        {
        }

        protected override Action<DomainModel.TestPoolEntry> GetEntityInitializer(SaveTestPoolEntryModel model, IEntityQuery query)
        {
            var entityPool = query.Get<DomainModel.TestPool>(model.TestPool);
            var entityQuestion = query.Get<TestQuestion>(model.TestQuestion);

            return g =>
            {
                g.Id = model.Id;
                g.Score = model.Score;
                g.ScoringStrategy = model.ScoringStrategy;
                g.TestQuestion = entityQuestion;
                g.TestPool = entityPool;
                //g.TestResults = new List<TestResult>();
            };
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected override bool ExistsInDatabase(SaveTestPoolEntryModel model)
        {
            return model.Id != 0;
        }

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected override object[] GetEntityKey(SaveTestPoolEntryModel model)
        {
            return new object[] { model.Id };
        }
    }
}
