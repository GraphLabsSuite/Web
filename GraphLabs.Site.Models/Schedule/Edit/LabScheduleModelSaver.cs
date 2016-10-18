using System;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Schedule.Edit
{
    class LabScheduleModelSaver : AbstractModelSaver<EditLabScheduleModel, AbstractLabSchedule>
    {
        public LabScheduleModelSaver(IOperationContextFactory<IGraphLabsContext> operationContextFactory) : base(operationContextFactory)
        {
        }

        protected override Action<AbstractLabSchedule> GetEntityInitializer(EditLabScheduleModel model, IEntityQuery query)
        {
            throw new NotImplementedException();
        }

        protected override bool ExistsInDatabase(EditLabScheduleModel model)
        {
            throw new NotImplementedException();
        }

        protected override object[] GetEntityKey(EditLabScheduleModel model)
        {
            throw new NotImplementedException();
        }
    }
}