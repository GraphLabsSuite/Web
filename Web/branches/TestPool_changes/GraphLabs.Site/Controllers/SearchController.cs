using GraphLabs.Site.Controllers.Attributes;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Schedule;
using GraphLabs.Site.Models.Schedule.Edit;
using GraphLabs.Site.Utils;
using System;
using System.Text.RegularExpressions;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class SearchController : GraphLabsController
    { 
        private readonly IListModelLoader _listModelLoader;
        private readonly IEntityBasedModelSaver<EditLabScheduleModelBase, AbstractLabSchedule> _modelSaver;
        private readonly IEntityBasedModelLoader<LabScheduleModel, AbstractLabSchedule> _modelLoader;
        private readonly IEditLabScheduleModelLoader _editModelLoader;
        // private readonly IEntityRemover<AbstractLabSchedule> _modelRemover;

        public SearchController(
            IListModelLoader listModelLoader,
            IEntityBasedModelSaver<EditLabScheduleModelBase, AbstractLabSchedule> modelSaver,
            IEntityBasedModelLoader<LabScheduleModel, AbstractLabSchedule> modelLoader,
            IEditLabScheduleModelLoader editModelLoader)//,
            //  IEntityRemover<AbstractLabSchedule> modelRemover)
        {
            _listModelLoader = listModelLoader;
            _modelSaver = modelSaver;
            _modelLoader = modelLoader;
            _editModelLoader = editModelLoader;
            //  _modelRemover = modelRemover;
        }
 
        public ActionResult Index(string message, string stringsearch)
        {
            // исключаем случай, когда в поисковую строку попали лишние начальные/конечные пробелы
            stringsearch.Trim(' ');
            // если задан пустой поисковый запрос, остаемся на странице, с которой осуществляли поиск
            if (String.IsNullOrEmpty(stringsearch))
            {
                Response.Write("<script>alert('Введен пустой поисковый запрос. Повторите попытку.')</script>");
                Response.Write("<script>history.go(-1);</script>");
                return null;
            }
            // проверяем, является ли строка для поиска датой
            // приводим предполагаемую дату к необходимому виду
            string proDate = stringsearch.Replace(' ', '/');
            proDate = stringsearch.Replace('.', '/');
            DateTime searchDate;
            // проверяем, можно ли привести измененную строку к формату даты
            try
            {
                searchDate = DateTime.Parse(proDate);
                // если успешно привели к формату даты
                var model = _listModelLoader
                 .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                 .FilterByDate(searchDate, searchDate);
                return View(model);
            }
            catch (FormatException)
            {
                // продолжим проверку на дату
                // проверяем, введен ли только год, для этого приводим строку к целому числу
                int proYear;
                if (Int32.TryParse(stringsearch, out proYear))
                {
                    proDate = "01/01/" + stringsearch;
                    searchDate = DateTime.Parse(proDate);
                    var model = _listModelLoader
                        .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                        .FilterByDate(searchDate, searchDate.AddYears(1));
                    return View(model);
                }
                else
                {
                    string[] search = stringsearch.Split(' ');
                    if (search.Length == 1)
                    {
                        // проверка: группа
                        string group1 = @"\w\d{2}-d{3}";
                        string group2 = @"\w\d{5}";
                        string name = @"\D+";
                        if (Regex.IsMatch(search[0], group1, RegexOptions.IgnoreCase))
                        {
                            string searchGroup = search[0].Remove(0, 1).Replace('-',' ');
                            var model = _listModelLoader
                                .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                                .FilterByUser(searchGroup);
                            return View(model);
                            //ФИЛЬТРАЦИЯ ПО ГРУППЕ
                        }
                        else if (Regex.IsMatch(search[0], group2, RegexOptions.IgnoreCase))
                        {
                            //string searchgroup = search[0].Remove(0, 1) + " " + sea
                            var model = _listModelLoader
                                .LoadListModel<LabScheduleListModel, LabScheduleModel>();
                            return View(model);
                            //ФИЛЬТРАЦИЯ ПО ГРУППЕ
                        }
                        // проверка: имя, фамилия или лабораторная работа
                        else if (Regex.IsMatch(search[0], name, RegexOptions.IgnoreCase)) {
                            ViewBag.Message = search[0];
                            var model = _listModelLoader
                                .LoadListModel<LabScheduleListModel, LabScheduleModel>();
                            return View(model);
                            // ФИЛЬТРАЦИЯ ПО ИМЕНИ ИЛИ ФАМИЛИИ
                        }
                        else
                        {
                            var model = _listModelLoader
                                .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                                .FilterByLabName(stringsearch);
                            return View(model);
                        }
                    }
                    else if (search.Length == 2)
                    {
                        // проверка: год и месяц
                        string year = @"\d{4}";
                        string chars = @"\D+";
                        string month1 = @"\d{1}";
                        string month2 = @"\d{2}";
                        string group1 = @"\w\d{2}";
                        string group2 = @"\d{3}";
                        if ((Regex.IsMatch(search[0], year, RegexOptions.IgnoreCase)) &&
                            ((Regex.IsMatch(search[1], chars, RegexOptions.IgnoreCase)) || (Regex.IsMatch(search[1], month1, RegexOptions.IgnoreCase)) || (Regex.IsMatch(search[1], month2, RegexOptions.IgnoreCase))))
                        {
                            switch (search[1].ToLower())
                            {
                                case "январь":
                                    search[1] = "01";
                                    break;
                                case "февраль":
                                    search[1] = "02";
                                    break;
                                case "март":
                                    search[1] = "03";
                                    break;
                                case "апрель":
                                    search[1] = "04";
                                    break;
                                case "май":
                                    search[1] = "05";
                                    break;
                                case "июнь":
                                    search[1] = "06";
                                    break;
                                case "июль":
                                    search[1] = "07";
                                    break;
                                case "август":
                                    search[1] = "08";
                                    break;
                                case "сентябрь":
                                    search[1] = "09";
                                    break;
                                case "октябрь":
                                    search[1] = "10";
                                    break;
                                case "ноябрь":
                                    search[1] = "11";
                                    break;
                                case "декабрь":
                                    search[1] = "12";
                                    break;
                                default:
                                    if (search[1].Length == 1)
                                    {
                                        search[1] = "0" + search[1];
                                    }
                                    break;
                            }
                            proDate = "01/" + search[1] + "/" + search[0];
                            searchDate = DateTime.Parse(proDate);
                            // если успешно привели к формату даты
                            ViewBag.Message = searchDate.ToString();
                            var model = _listModelLoader
                             .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                             .FilterByDate(searchDate, searchDate.AddMonths(1));
                            return View(model);
                        }
                        else if ((Regex.IsMatch(search[1], year, RegexOptions.IgnoreCase)) &&
                            ((Regex.IsMatch(search[0], chars, RegexOptions.IgnoreCase)) || (Regex.IsMatch(search[0], month1, RegexOptions.IgnoreCase)) || (Regex.IsMatch(search[0], month2, RegexOptions.IgnoreCase))))
                        {
                            switch (search[0].ToLower())
                            {
                                case "январь":
                                    search[0] = "01";
                                    break;
                                case "февраль":
                                    search[0] = "02";
                                    break;
                                case "март":
                                    search[0] = "03";
                                    break;
                                case "апрель":
                                    search[0] = "04";
                                    break;
                                case "май":
                                    search[0] = "05";
                                    break;
                                case "июнь":
                                    search[0] = "06";
                                    break;
                                case "июль":
                                    search[0] = "07";
                                    break;
                                case "август":
                                    search[0] = "08";
                                    break;
                                case "сентябрь":
                                    search[0] = "09";
                                    break;
                                case "октябрь":
                                    search[0] = "10";
                                    break;
                                case "ноябрь":
                                    search[0] = "11";
                                    break;
                                case "декабрь":
                                    search[0] = "12";
                                    break;
                                default:
                                    if (search[0].Length == 1)
                                    {
                                        search[0] = "0" + search[1];
                                    }
                                    break;
                            }
                            proDate = "01/" + search[0] + "/" + search[1];
                            searchDate = DateTime.Parse(proDate);
                            // если успешно привели к формату даты
                            ViewBag.Message = searchDate.ToString();
                            var model = _listModelLoader
                             .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                             .FilterByDate(searchDate, searchDate.AddMonths(1));
                            return View(model);
                        }
                        // проверка: группа
                        else if ((Regex.IsMatch(search[0], group1, RegexOptions.IgnoreCase)) && (Regex.IsMatch(search[1], group2, RegexOptions.IgnoreCase)))
                        {
                            ViewBag.Message = search[0] + search[1];
                            var model = _listModelLoader
                                .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                                .FilterByUser(stringsearch);
                            return View(model);
                        }
                        // проверка: имя, фамилия, лабораторная работа
                        else if ((Regex.IsMatch(search[0], chars, RegexOptions.IgnoreCase)) && (Regex.IsMatch(search[1], chars, RegexOptions.IgnoreCase)))
                        {
                            ViewBag.Message = search[0] + search[1];
                            var model = _listModelLoader
                                .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                                .FilterByUser(stringsearch);
                            return View(model);
                        }
                        else
                        {
                            var model = _listModelLoader
                                .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                                .FilterByLabName(stringsearch);
                            return View(model);
                        }
                    }
                    else if (search.Length == 3)
                    {
                        string year = @"\d{4}";
                        string chars = @"\D+";
                        string day1 = @"\d{1}";
                        string day2 = @"\d{2}";
                        // проверка: имя, фамилия, лабораторная работа
                        if ((Regex.IsMatch(search[0], chars, RegexOptions.IgnoreCase)) && (Regex.IsMatch(search[1], chars, RegexOptions.IgnoreCase)) && (Regex.IsMatch(search[2], chars, RegexOptions.IgnoreCase)))
                        {
                            ViewBag.Message = search[0] + search[1] + search[2];
                            var model = _listModelLoader
                                .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                                .FilterByUser(stringsearch);
                            return View(model);
                        }
                        else if (((Regex.IsMatch(search[0], day1, RegexOptions.IgnoreCase)) || (Regex.IsMatch(search[0], day2, RegexOptions.IgnoreCase))) 
                            && (Regex.IsMatch(search[1], chars, RegexOptions.IgnoreCase)) && (Regex.IsMatch(search[2], year, RegexOptions.IgnoreCase)))
                        {
                            switch (search[0].ToLower())
                            {
                                case "января":
                                    search[1] = "01";
                                    break;
                                case "февраля":
                                    search[1] = "02";
                                    break;
                                case "марта":
                                    search[1] = "03";
                                    break;
                                case "апреля":
                                    search[1] = "04";
                                    break;
                                case "мая":
                                    search[1] = "05";
                                    break;
                                case "июня":
                                    search[1] = "06";
                                    break;
                                case "июля":
                                    search[1] = "07";
                                    break;
                                case "августа":
                                    search[1] = "08";
                                    break;
                                case "сентября":
                                    search[1] = "09";
                                    break;
                                case "октября":
                                    search[1] = "10";
                                    break;
                                case "ноября":
                                    search[1] = "11";
                                    break;
                                case "декабря":
                                    search[1] = "12";
                                    break;
                                case "январь":
                                    search[1] = "01";
                                    break;
                                case "февраль":
                                    search[1] = "02";
                                    break;
                                case "март":
                                    search[1] = "03";
                                    break;
                                case "апрель":
                                    search[1] = "04";
                                    break;
                                case "май":
                                    search[1] = "05";
                                    break;
                                case "июнь":
                                    search[1] = "06";
                                    break;
                                case "июль":
                                    search[1] = "07";
                                    break;
                                case "август":
                                    search[1] = "08";
                                    break;
                                case "сентябрь":
                                    search[1] = "09";
                                    break;
                                case "октябрь":
                                    search[1] = "10";
                                    break;
                                case "ноябрь":
                                    search[1] = "11";
                                    break;
                                case "декабрь":
                                    search[1] = "12";
                                    break;
                                default:
                                    search[1] = "error";
                                    break;
                            }
                            if (search[0].Equals("error"))
                            {
                                var model = _listModelLoader
                                    .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                                    .FilterByLabName(stringsearch);
                                return View(model);
                            }
                            else
                            {
                                if (search[0].Length == 1)
                                {
                                    search[0] = "0" + search[0];
                                }
                                proDate = search[0] + "/" + search[1] + "/" + search[2];
                                searchDate = DateTime.Parse(proDate);
                                // если успешно привели к формату даты
                                ViewBag.Message = searchDate.ToString();
                                var model = _listModelLoader
                                 .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                                 .FilterByDate(searchDate, searchDate.AddMonths(1));
                                return View(model);
                            }
                        }
                        else
                        {
                            
                            var model = _listModelLoader
                                .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                                .FilterByLabName(stringsearch);
                            return View(model);
                        }
                    }
                    else
                    {
                        var model = _listModelLoader
                            .LoadListModel<LabScheduleListModel, LabScheduleModel>()
                            .FilterByLabName(stringsearch);
                        return View(model);
                    }
                }
            }
        }

        public ActionResult CreateSchedule(EditLabScheduleModelBase.Kind kind)
        {
            return View(_editModelLoader.CreateEmptyModel(kind));
        }

        [HttpPost]
        public ActionResult CreateSchedule([ModelBinder(typeof(SmartModelBinder))]EditLabScheduleModelBase schedule)
        {
            if (ModelState.IsValid)
            {
                _modelSaver.CreateOrUpdate(schedule);
                ViewBag.Message = "Расписание создано";
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Невозможно сохранить строку расписания";
            return View(schedule);
        }

        public ActionResult EditSchedule(long id = 0)
        {
            return View(_editModelLoader.Load(id));
        }

        [HttpPost]
        public ActionResult EditSchedule([ModelBinder(typeof(SmartModelBinder))]EditLabScheduleModelBase schedule)
        {
            if (ModelState.IsValid)
            {
                _modelSaver.CreateOrUpdate(schedule);
                ViewBag.Message = "Изменения сохранены";
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Невозможно обновить строку расписания";
            return View(schedule);
        }

   /*     public ActionResult Delete(long id = 0)
        {
            return View(_modelRemover.Remove(id));
        }

        [HttpPost]
        public ActionResult Delete([ModelBinder(typeof(SmartModelBinder))]EditLabScheduleModelBase schedule)
        {
            if (ModelState.IsValid)
            {
                _modelSaver.CreateOrUpdate(schedule);
                ViewBag.Message = "Изменения сохранены";
                return RedirectToAction("Index");
            }
            ViewBag.Message = "Невозможно обновить строку расписания";
                  return View(schedule);
         }*/
    }
}