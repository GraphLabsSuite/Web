using System;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.LabWorks
{
    /// <summary> Сервис сохранения лабораторных работ </summary>
    sealed class LabWorkModelSaver : AbstractModelSaver<LabWorkModel, LabWork>
    {
        /// <summary> Сервис сохранения лабораторных работ </summary>
        public LabWorkModelSaver(IOperationContextFactory<IGraphLabsContext> operationContextFactory)
            : base(operationContextFactory)
        {
        }

        protected override Action<LabWork> GetEntityInitializer(LabWorkModel model, IEntityQuery query)
        {
            return l =>
            {
                l.Name = model.Name;
                l.AcquaintanceFrom = model.AcquaintanceFrom;
                l.AcquaintanceTill = model.AcquaintanceTill;
                l.LabVariants = model.LabVariants;
            };
        }

        /// <summary> Существует ли соответствующая запись в БД? </summary>
        /// <remarks> При реализации - просто проверить ключ, в базу лазить НЕ НАДО </remarks>
        protected override bool ExistsInDatabase(LabWorkModel model)
        {
            return model.Id != 0;
        }

        /// <summary> При реализации возвращает массив первичных ключей сущности </summary>
        protected override object[] GetEntityKey(LabWorkModel model)
        {
            return new object[] {model.Id};
        }
    }
}