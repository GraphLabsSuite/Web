using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef.Services;

namespace GraphLabs.Dal.Ef.Extensions
{
    /// <summary> Расширения для сервиса системного времени </summary>
    public static class SystemDateExtensions
    {
        /// <summary> Возвращает текущий семестр (осень/весна) </summary>
        /// <remarks> Считаем, что весенний - с февраля по август, осенний - с сентября по январь.</remarks>
        public static Term GetTerm(this ISystemDateService service)
        {
            var month = service.GetDate().Month;

            if (month >= 2 && month <= 8)
                return Term.Spring;
            else
            {
                return Term.Autumn;
            }
        }
    }
}
