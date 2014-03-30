using System.Web.Mvc;

namespace GraphLabs.Site.App_Start
{
    /// <summary> Настройка фильтров </summary>
    public class FilterConfig
    {
        /// <summary> Настройка фильтров </summary>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute()); // Ошибки
        }
    }
}