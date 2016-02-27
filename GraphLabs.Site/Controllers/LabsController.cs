using GraphLabs.DomainModel.EF;
using GraphLabs.DomainModel.EF.Repositories;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Controllers.LabWorks;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using GraphLabs.DomainModel.EF.Contexts;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class LabsController : GraphLabsController
    {
        #region Зависимости

        private readonly ILabRepository _labRepository;
        private readonly ITasksContext _tasksContext;

        #endregion

        public LabsController(ILabRepository labRepository, ITasksContext tasksContext)
        {
            _labRepository = labRepository;
            _tasksContext = tasksContext;
        }

        #region Отображение списка лабораторных работ

        public ActionResult Index()
        {
            return View(_labRepository.GetLabWorks());
        }

        [HttpPost]
        public JsonResult GetLabInfo(int Id)
        {
            var lab = _labRepository.GetLabWorkById(Id);

            return Json(new JSONResultLabInfo(lab));
        }

        #endregion        

        #region Создание и редактирование лабораторной работы

        public ActionResult Create(long id = 0)
        {
            return View( new CreateLabModel(id, _tasksContext.Tasks.ToArray()) );
        }

        [HttpPost]
        public JsonResult LabWorkCreate(string Name, string DateFrom, string DateTo, string JsonArr)
        {
            if (_labRepository.CheckLabWorkExist(Name))
            {
                return Json(new JSONResultCreateLab( ResponseConstants.LabWorkExistErrorSystemName, Name ));
			};

			LabWork lab = new LabWork();
			lab.Name = Name;
			lab.AcquaintanceFrom = ParseDate.Parse(DateFrom);
			lab.AcquaintanceTill = ParseDate.Parse(DateTo);

			_labRepository.SaveLabWork(lab);
			_labRepository.SaveLabEntries(lab.Id, JsonConvert.DeserializeObject<long[]>(JsonArr));
			_labRepository.DeleteExcessTaskVariantsFromLabVariants(lab.Id);

			return Json(new JSONResultCreateLab(ResponseConstants.LabWorkSuccessCreateSystemName, Name, lab.Id));
        }

        [HttpPost]
        public JsonResult LabWorkEdit(string Name, string DateFrom, string DateTo, string JsonArr, long id)
        {
			if (_labRepository.CheckLabWorkExist(Name) && (_labRepository.GetLabWorkIdByName(Name) != id))
			{
				return Json(new JSONResultCreateLab(ResponseConstants.LabWorkExistErrorSystemName, Name));
			};

			LabWork lab = _labRepository.GetLabWorkById(id);
			lab.Name = Name;
			lab.AcquaintanceFrom = ParseDate.Parse(DateFrom);
			lab.AcquaintanceTill = ParseDate.Parse(DateTo);

			_labRepository.DeleteEntries(id);
			lab.LabEntries.Clear();

			_labRepository.ModifyLabWork(lab);
			_labRepository.SaveLabEntries(lab.Id, JsonConvert.DeserializeObject<long[]>(JsonArr));
			_labRepository.DeleteExcessTaskVariantsFromLabVariants(lab.Id);

			return Json(new JSONResultCreateLab(ResponseConstants.LabWorkSuccessEditSystemName, Name, lab.Id));
        }
        
        //В id передается результат запроса
        [HttpPost]
        public JsonResult EditLab(long Id)
        {
            var lab = _labRepository.GetLabWorkById(Id);
            
            return Json( new CreateLabModel(lab) );
        }

        #endregion

        #region Создание и редактирование варианта лабораторной работы

        public ActionResult IndexCreateVariant(long labId, long varId = 0)
        {
            var lab = _labRepository.GetLabWorkById(labId);

            return View( new CreateLabVariantModel(lab, varId) );
        }

		[HttpPost]
		public JsonResult CreateVariant(long Id, string Number, string JsonArr, bool IntrVar)
		{
			LabWork lab = _labRepository.GetLabWorkById(Id);

			if (_labRepository.CheckLabVariantExist(Id, Number))
			{
				return Json(ResponseConstants.LabVariantNameCollisionSystemName);
			}

			LabVariant labVar = new LabVariant();
			labVar.LabWork = lab;
			labVar.Number = Number;
			labVar.IntroducingVariant = IntrVar;
			labVar.Version = 1;
			labVar.TaskVariants = MakeTaskVariantsList(JsonConvert.DeserializeObject<long[]>(JsonArr));

			try
			{
				_labRepository.SaveLabVariant(labVar);
			}
			catch (Exception)
			{
				return Json(ResponseConstants.LabVariantSaveErrorSystemName);
			}

			return Json(ResponseConstants.LabVariantSaveSuccessSystemName);
		}

		[HttpPost]
		public JsonResult EditVariant(string Number, string JsonArr, bool IntrVar, long variantId)
		{
			LabVariant labVar = _labRepository.GetLabVariantById(variantId);
			long labId = labVar.LabWork.Id;

			if (_labRepository.CheckLabVariantExist(labId, Number) && (_labRepository.GetLabVariantIdByNumber(labId, Number) != variantId))
			{
				return Json(ResponseConstants.LabVariantNameCollisionSystemName);
			}

			labVar.Number = Number;
			labVar.IntroducingVariant = IntrVar;
			labVar.Version += 1;
			labVar.TaskVariants.Clear();
			labVar.TaskVariants = MakeTaskVariantsList(JsonConvert.DeserializeObject<long[]>(JsonArr));

			try
			{
				_labRepository.ModifyLabVariant(labVar);
			}
			catch (Exception)
			{
				return Json(ResponseConstants.LabVariantModifyErrorSystemName);
			}

			return Json(ResponseConstants.LabVariantModifySuccessSystemName);
		}

		/// <summary> Создает список вариантов заданий из массива id </summary>
		private List<TaskVariant> MakeTaskVariantsList(long[] taskIds)
		{
			var result = new List<TaskVariant>();

			foreach (var tv in taskIds.Distinct().Select(tId => _tasksContext.TaskVariants.Single(tv => tv.Id == tId)))
			{
				result.Add(tv);
			}
			return result;
		}

        //В id передается результат, в Name - номер варианта
        [HttpPost]
        public JsonResult GetVariantInfo(long varId)
        {
            var variant = _labRepository.GetLabVariantById(varId);
            return Json(new JSONResultEditVariant(variant) );
        }

        #endregion
    }
}