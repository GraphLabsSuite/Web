using System;
using System.Linq.Expressions;
using GraphLabs.Dal.Ef.Extensions;
using GraphLabs.DomainModel;
using GraphLabs.Site.Core.Filters;
using GraphLabs.Site.Models.Infrastructure;

namespace GraphLabs.Site.Models.Schedule
{
    /// <summary> Модель <see cref="AbstractLabSchedule"/> </summary>
    public class LabScheduleModel : AbstractFilterableModel<AbstractLabSchedule>, IEntityBasedModel<AbstractLabSchedule>
    {
        public long Id { get; set; }

        [StringFilter("Название лабораторной работы")]
        public string LabName { get; set; }

        [StringFilter("C даты начала")]
        public DateTime DateFrom { get; set; }
        
        [StringFilter("До даты окончания")]
        public DateTime DateTill { get; set; }

        [BoundedFilter("Режим выполнения", new Object[] {"", "Контрольный", "Ознакомительный"})]
        public LabExecutionMode Mode { get; set; }

        [StringFilter("Выполняет")]
        public string Doer { get; set; }

        private static string extractLastName(string doer)
        {
            if (doer == null) return null;
            return doer.Split(' ')[0];
        }
        
        public static Expression<Func<AbstractLabSchedule, bool>> CreateFilter(
            FilterParams<LabScheduleModel> filterParams)
        {
            var name = filterParams.GetStringParam(nameof(LabName));
            var mode = LabModeExtensions.GetValueByString((string) filterParams.GetBoundedParam(nameof(Mode)));
            var df = filterParams.GetDateTimeParam(nameof(DateFrom));
            var dt = filterParams.GetDateTimeParam(nameof(DateTill));
            var doer = filterParams.GetStringParam(nameof(Doer));
            var lastname = extractLastName(doer);
            return l => (name == null || name.ToLower().Equals(l.LabWork.Name.ToLower()))
                        && (mode == null || l.Mode == mode)
                        && (df == null || df.Value.CompareTo(l.DateFrom) <= 0)
                        && (dt == null || dt.Value.CompareTo(l.DateTill) >= 0)
                        && ((doer == null) 
                            || ((l is IndividualLabSchedule) && (l as IndividualLabSchedule).Student.Surname.ToLower().Equals(lastname.ToLower()))
                            || ((l is GroupLabSchedule) && (l as GroupLabSchedule).Group.Name.ToLower().Equals(doer.ToLower())));
        }
    }
}