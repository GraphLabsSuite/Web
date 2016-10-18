using System;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Core.OperationContext;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Schedule.Edit
{
    /// <summary> Сохранятор моделей <see cref="AbstractLabSchedule"/> </summary>
    class EditLabScheduleModelSaver : AbstractModelSaver<EditLabScheduleModel, AbstractLabSchedule>
    {
        public EditLabScheduleModelSaver(IOperationContextFactory<IGraphLabsContext> operationContextFactory) : base(operationContextFactory)
        {
        }

        protected override Action<AbstractLabSchedule> GetEntityInitializer(EditLabScheduleModel model, IEntityQuery query)
        {
            return sch =>
            {
                sch.DateFrom = model.DateFrom;
                sch.DateTill = model.DateTill;
                sch.Mode = model.Mode;

                var groupSch = sch as GroupLabSchedule;
                if (groupSch != null)
                {
                    groupSch.Group = query.Get<Group>(model.SelectedDoerId);
                    return;
                }

                var studentSch = sch as IndividualLabSchedule;
                if (studentSch != null)
                {
                    studentSch.Student = query.Get<Student>(model.SelectedDoerId);
                }

                throw new ArgumentOutOfRangeException($"Сущность типа {sch.GetType()} не поддерживается.");
            };
        }

        protected override Type GetEntityType(EditLabScheduleModel model)
        {
            switch (model.SelectedDoerKind)
            {
                case EditLabScheduleModel.DoerKind.Group:
                    return typeof (GroupLabSchedule);
                case EditLabScheduleModel.DoerKind.Student:
                    return typeof (IndividualLabSchedule);
                default:
                    throw new ArgumentOutOfRangeException($"Поддержка {model.SelectedDoerKind} не реализована.");
            }
        }

        protected override bool ExistsInDatabase(EditLabScheduleModel model)
        {
            return model.Id > 0;
        }

        protected override object[] GetEntityKey(EditLabScheduleModel model)
        {
            return new object[] { model.Id };
        }
    }
}