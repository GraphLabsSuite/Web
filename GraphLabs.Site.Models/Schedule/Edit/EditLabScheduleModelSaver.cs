using System;
using System.Diagnostics.Contracts;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;
using JetBrains.Annotations;

namespace GraphLabs.Site.Models.Schedule.Edit
{
    /// <summary> Сохранятор моделей <see cref="AbstractLabSchedule"/> </summary>
    [UsedImplicitly]
    class EditLabScheduleModelSaver : AbstractModelSaver<EditLabScheduleModelBase, AbstractLabSchedule>
    {
        public EditLabScheduleModelSaver(IOperationContextFactory<IGraphLabsContext> operationContextFactory) : base(operationContextFactory)
        {
        }

        protected override Action<AbstractLabSchedule> GetEntityInitializer(EditLabScheduleModelBase model, IEntityQuery query)
        {
            return sch =>
            {
                sch.DateFrom = model.DateFrom;
                sch.DateTill = model.DateTill;
                sch.Mode = model.Mode;

                var groupSch = sch as GroupLabSchedule;
                if (groupSch != null)
                {
                    Contract.Assert(model.ScheduleKind == EditLabScheduleModelBase.Kind.Group);
                    groupSch.Group = query.Get<Group>(model.GetDoerId());
                    return;
                }

                var studentSch = sch as IndividualLabSchedule;
                if (studentSch != null)
                {
                    Contract.Assert(model.ScheduleKind == EditLabScheduleModelBase.Kind.Individual);
                    studentSch.Student = query.Get<Student>(model.GetDoerId());
                    return;
                }

                throw new ArgumentOutOfRangeException($"Сущность типа {sch.GetType()} не поддерживается.");
            };
        }

        protected override Type GetEntityType(EditLabScheduleModelBase model)
        {
            switch (model.ScheduleKind)
            {
                case EditLabScheduleModelBase.Kind.Group:
                    return typeof(GroupLabSchedule);
                case EditLabScheduleModelBase.Kind.Individual:
                    return typeof(IndividualLabSchedule);
                default:
                    throw new ArgumentOutOfRangeException($"Поддержка {model.ScheduleKind} не реализована.");
            }
        }

        protected override bool ExistsInDatabase(EditLabScheduleModelBase model)
        {
            return model.Id > 0;
        }

        protected override object[] GetEntityKey(EditLabScheduleModelBase model)
        {
            return new object[] { model.Id };
        }
    }
}