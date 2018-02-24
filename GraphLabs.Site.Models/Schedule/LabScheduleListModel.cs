using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System;

namespace GraphLabs.Site.Models.Schedule
{
    /// <summary> Модель расписания (списка) </summary>
    public class LabScheduleListModel : ListModelBase<LabScheduleModel>,
        IFilterableByDate<LabScheduleListModel, LabScheduleModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<LabScheduleModel, AbstractLabSchedule> _modelLoader;

        public LabScheduleListModel(
            IEntityQuery query,
            IEntityBasedModelLoader<LabScheduleModel, AbstractLabSchedule> modelLoader)
        {
            _query = query;
            _modelLoader = modelLoader;
        }

        private DateTime? _dateFrom;
        private DateTime? _dateTill;

        public LabScheduleListModel FilterByDate(DateTime? from, DateTime? till)
        {
            if (from.HasValue && till.HasValue && till < from)
                throw new ArgumentOutOfRangeException();

            _dateFrom = from?.Date;
            _dateTill = till?.Date.AddDays(1).AddMilliseconds(-1);

            return this;
        }

        protected override LabScheduleModel[] LoadItems()
        {
            return _query
                .OfEntities<AbstractLabSchedule>()
                .OrderBy(m => m.DateFrom)
                .ThenBy(m => m.DateTill)
                .Where(m =>
                   (!_dateFrom.HasValue || _dateFrom <= m.DateTill)
                && (!_dateTill.HasValue || _dateTill >= m.DateFrom))
                .ToArray()
                .Select(_modelLoader.Load)
                .ToArray();
        }
    }
}