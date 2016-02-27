using System.Diagnostics.Contracts;
using GraphLabs.DomainModel.EF.Services;

namespace GraphLabs.DomainModel.EF.Extensions
{
    /// <summary> Методы расширения для групп </summary>
    public static class GroupExtensions
    {
        

        /// <summary> Возвращает имя группы </summary>
        public static string GetName(this Group group, ISystemDateService dateService)
        {
            Contract.Requires(group != null);
            Contract.Requires(dateService != null);

            var currentDate = dateService.GetDate();
            var currentTerm = dateService.GetTerm();

            var delta = (currentDate.Month != 1 ? currentDate.Year : currentDate.Year - 1) - group.FirstYear;
            var termPrefix = delta * 2;
            if (currentTerm == Term.Autumn)
                termPrefix += 1;

            return string.Format("К{0:00}-{1}", termPrefix, group.Number);
        }
    }
}
