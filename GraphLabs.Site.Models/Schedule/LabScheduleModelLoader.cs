using System;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Startpage
{
    /// <summary> Загрузчик модели <see cref="LabStartpageModel"/> </summary>
    class LabScheduleModelLoader : AbstractModelLoader<LabStartpageModel, AbstractLabSchedule>
    {
        public LabScheduleModelLoader(IEntityQuery query) : base(query)
        {
        }

        public override LabStartpageModel Load(AbstractLabSchedule entity)
        {
            var model = new LabStartpageModel
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
                model.Doer = groupSchedule.Group.Name;
                return model;
            }

            throw new ArgumentOutOfRangeException($"Загрузка модели строки расписания для типа {entity.GetType()} не реализована.");
        }
    }
}