using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Controllers.LabWorks;
using GraphLabs.Site.Logic.LabsLogic;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class LabsController : GraphLabsController
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();
        private LabsLogic logic = new LabsLogic();

        #region Зависимости

        private ILabRepository _labRepository
        {
            get { return DependencyResolver.GetService<ILabRepository>(); }
        }

        private ITaskRepository _taskRepository
        {
            get { return DependencyResolver.GetService<ITaskRepository>(); }
        }

        #endregion

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
            return View( new CreateLabModel(id, _taskRepository.GetAllTasks()) );
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

        public ActionResult CreateVariant(long labId = 0, long varId = 0)
        {
            //this.AllowAnonymous();
            var lab = logic.GetLabWorkById(labId);
            if (lab == null)
            {
                return HttpNotFound();
            }            
            return View( new CreateLabVariantModel(lab, varId) );
        }
        
        [HttpPost]
        public string CreateVariant(int Id, string Number, string JsonArr, bool IntrVar, int variantId = 0)
        {
            const string SuccesfulCreating = "0";
            const string LabWorkNotFoundError = "1";
            const string NameCollisionError = "2";
            const string LabVariantNotFoundError = "3";
            const string SuccesfulUpdating = "4";
            const string TaskVariantNotFoundError = "5";
            const string UnknownSavingError = "6";
            //this.AllowAnonymous(_ctx);

            LabWork lab = logic.GetLabWorkById(Id);
            if (lab == null)
            {
                return LabWorkNotFoundError;
            }

            if (logic.ExistedLabVariantsCount(Number, lab.Id, variantId) != 0)
            {
                return NameCollisionError;
            }

            LabVariant labVar = logic.CreateOrGetLabVariantDependingOnId(variantId);
            if (labVar == null)
            {
                return LabVariantNotFoundError;
            }

            labVar.LabWork = lab;
            labVar.Number = Number;
            labVar.IntroducingVariant = IntrVar;
            if (IsNewLabVar(variantId))
            {
                labVar.Version = 1;
            }
            else
            {
                ++labVar.Version;
            }

            try
            {
                labVar.TaskVariants.Clear();
                labVar.TaskVariants = logic.MakeTaskVariantsList(JsonConvert.DeserializeObject<int[]>(JsonArr));
            }
            catch (Exception)
            {
                return TaskVariantNotFoundError;
            }

            try
            {
                logic.SaveLabVariant(labVar, IsNewLabVar(variantId));
            }
            catch (Exception)
            {
                return UnknownSavingError;
            }

            if (IsNewLabVar(variantId))
            {
                return SuccesfulCreating;
            }
            else
            {
                return SuccesfulUpdating;
            }
        }

        /// <summary> Проверяет, что обрабатывается новый вариант лабораторной работы </summary>
        private bool IsNewLabVar(long Id)
        {
            return (Id == 0);
        }

        //В id передается результат, в Name - номер варианта
        [HttpPost]
        public string EditVariant(long varId)
        {
            const int VariantNotFoundError = 1;
            const int SuccesfulWork = 0;
            //this.AllowAnonymous(_ctx);
            var variant = logic.GetLabVariantById(varId);
            if (variant == null)
            {
                return JsonConvert.SerializeObject( new JSONResultEditVariant(VariantNotFoundError) );
            }
            return JsonConvert.SerializeObject(new JSONResultEditVariant(SuccesfulWork, variant) );
        }

        #endregion
    }
}