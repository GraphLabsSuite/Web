using System;
using GraphLabs.Dal.Ef.Extensions;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Schedule
{
    /// <summary> Загрузчик модели <see cref="LabScheduleModel"/> </summary>
    class LabScheduleModelLoader : AbstractModelLoader<LabScheduleModel, AbstractLabSchedule>
    {
        private readonly ISystemDateService _systemDate;

        public LabScheduleModelLoader(IEntityQuery query, ISystemDateService systemDate) : base(query)
        {
            _systemDate = systemDate;
        }

        public override LabScheduleModel Load(AbstractLabSchedule entity)
        {
            var model = new LabScheduleModel
            {
                Id = entity.Id,
                DateFrom = entity.DateFrom,
                DateTill = entity.DateTill,
                Mode = entity.Mode,
                LabName = entity.LabWork.Name
            };

            var studentSchedule = entity as IndividualLabSchedule;
            if (studentSchedule != null)
            {
                model.Doer = studentSchedule.Student.GetShortName();
                return model;
            }

            var groupSchedule = entity as GroupLabSchedule;
            if (groupSchedule != null)
            {
                model.Doer = groupSchedule.Group.GetName(_systemDate);
                return model;
            }

            throw new ArgumentOutOfRangeException($"Загрузка модели строки расписания для типа {entity.GetType()} не реализована.");
        }
    }
}