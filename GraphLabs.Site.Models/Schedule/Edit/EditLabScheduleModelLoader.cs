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
    class EditLabScheduleModelLoader : AbstractModelLoader<EditLabScheduleModelBase, AbstractLabSchedule>, IEditLabScheduleModelLoader
    {
        private readonly ISystemDateService _systemDate;

        public EditLabScheduleModelLoader(IEntityQuery query, ISystemDateService systemDate) : base(query)
        {
            _systemDate = systemDate;
        }

        public override EditLabScheduleModelBase Load(AbstractLabSchedule entity)
        {
            EditLabScheduleModelBase model = null;
            var studentSchedule = entity as IndividualLabSchedule;
            if (studentSchedule != null)
            {
                model = CreateEmptyModel(EditLabScheduleModelBase.Kind.Individual);
            }
            var groupSchedule = entity as GroupLabSchedule;
            if (groupSchedule != null)
            {
                model = CreateEmptyModel(EditLabScheduleModelBase.Kind.Group);
            }

            if (model == null)
                throw new ArgumentOutOfRangeException($"Загрузка модели строки расписания для типа {entity.GetType()} не реализована.");

            model.Id = entity.Id;
            model.DateFrom = entity.DateFrom;
            model.DateTill = entity.DateTill;
            model.Mode = entity.Mode;

            return model;
        }

        public EditLabScheduleModelBase CreateEmptyModel(EditLabScheduleModelBase.Kind kind)
        {
            EditLabScheduleModelBase model;
            switch (kind)
            {
                case EditLabScheduleModelBase.Kind.Individual:
                    model = new EditIndividualLabScheduleModel
                    {
                        Doers = _query.OfEntities<Student>()
                            .Where(s => !s.IsDismissed && s.IsVerified)
                            .ToArray()
                            .ToDictionary(s => s.Id, s => $"[{s.Group.GetName(_systemDate)}] {s.GetShortName()}")
                    };
                    break;
                case EditLabScheduleModelBase.Kind.Group:
                    model = new EditGroupLabScheduleModel
                    {
                        Doers = _query.OfEntities<Group>()
                            .ToArray()
                            .ToDictionary(g => g.Id, g => g.GetName(_systemDate))
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind));
            }

            model.DateFrom = _systemDate.Now();
            model.DateTill = _systemDate.Now();

            return model;
        }
    }
}