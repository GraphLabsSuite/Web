using System;
using System.Linq;
using GraphLabs.Dal.Ef.Extensions;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Schedule.Edit
{
    /// <summary> Загрузчик модели <see cref="EditLabScheduleModel"/> </summary>
    class EditLabScheduleModelLoader : AbstractModelLoader<EditLabScheduleModel, AbstractLabSchedule>
    {
        private readonly ISystemDateService _systemDate;

        public EditLabScheduleModelLoader(IEntityQuery query, ISystemDateService systemDate) : base(query)
        {
            _systemDate = systemDate;
        }

        public override EditLabScheduleModel Load(AbstractLabSchedule entity)
        {
            var model = new EditLabScheduleModel
            {
                DateFrom = entity.DateFrom,
                DateTill = entity.DateTill,
                Mode = entity.Mode,
                CanChangeDoerKind = false
            };

            var studentSchedule = entity as IndividualLabSchedule;
            if (studentSchedule != null)
            {
                model.Students = _query.OfEntities<Student>()
                    .Where(s => !s.IsDismissed && s.IsVerified)
                    .ToArray()
                    .ToDictionary(s => s.Id, s => s.GetShortName());
                return model;
            }

            var groupSchedule = entity as GroupLabSchedule;
            if (groupSchedule != null)
            {
                model.Groups = _query.OfEntities<Group>()
                .ToArray()
                .ToDictionary(g => g.Id, g => g.GetName(_systemDate));
            }

            throw new ArgumentOutOfRangeException($"Загрузка модели строки расписания для типа {entity.GetType()} не реализована.");
        }
    }
}