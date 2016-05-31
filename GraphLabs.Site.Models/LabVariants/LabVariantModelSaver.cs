using System;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabVariants
{
    /// <summary> Сервис сохранения вариантов лабораторных работ </summary>
    sealed class LabVariantModelSaver : AbstractModelSaver<LabVariantModel, DomainModel.LabVariant>
    {
        /// <summary> Сервис сохранения вариантов лабораторных работ </summary>
        public LabVariantModelSaver(IOperationContextFactory<IGraphLabsContext> operationContextFactory)
            : base(operationContextFactory)
        {
        }

        protected override Action<DomainModel.LabVariant> GetEntityInitializer(LabVariantModel model, IEntityQuery query)
        {
            return v =>
            {
                v.LabWork = (DomainModel.LabWork) query.OfEntities<DomainModel.LabWork>().Select(l => l.Id == model.LabWorkId);
                v.IntroducingVariant = model.IntroducingVariant;
                v.Number = model.Number;
                v.Results = model.Results;
                v.TaskVariants = model.TaskVariants;
                v.TestQuestions = model.TestQuestions;
                v.Version = model.Version;
            };
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected override bool ExistsInDatabase(LabVariantModel model)
        {
            return model.Id != 0;
        }

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected override object[] GetEntityKey(LabVariantModel model)
        {
            return new object[] { model.Id };
        }
    }
}