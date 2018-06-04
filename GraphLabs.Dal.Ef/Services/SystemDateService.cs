using System;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.Dal.Ef.Services;

namespace GraphLabs.Dal.Ef.Services
{
    /// <summary> Системное время </summary>
    public class SystemDateService : ISystemDateService
    {
        private readonly GraphLabsContext _context;

        public SystemDateService(GraphLabsContext context)
        {
            _context = context;
        }

        /// <summary> Возвращает текущее системное время </summary>
        public DateTime GetDate()
        {
            return _context.Settings.Single().SystemDate.Date;
        }

        /// <summary> Устанавливает время системы </summary>
        public void SetDate(DateTime newDateTime)
        {
            var currentDate = GetDate();
            var newDate = newDateTime.Date;

            Guard.IsTrueAssertion("Новая дата должна быть больше предыдущей.", newDate > currentDate); 

            var settings = _context.Settings.Single();
            settings.SystemDate = newDate;
            _context.SaveChanges();
        }

        /// <summary> Текущий момент времени (не учётный, а реальный) </summary>
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}
