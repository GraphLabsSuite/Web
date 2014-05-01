using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Logic.Labs;
using GraphLabs.Site.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
using GraphLabs.DomainModel.Repositories;
using Newtonsoft.Json;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class LabWorkExecutionController : GraphLabsController
    {
        private ILabRepository LabRepository
        {
            get { return DependencyResolver.GetService<ILabRepository>(); }
        }

        private ILabExecutionEngine LabExecutionEngine
        {
            get { return DependencyResolver.GetService<ILabExecutionEngine>(); }
        }

        public ActionResult Index(long labId, long labVarId)
        {
            if (!LabExecutionEngine.IsLabVariantCorrect(labVarId))
            {
                ViewBag.Message = "Вариант лабораторной работы не завершен";
                return View("LabWorkExecutionError");
            }

            string labName = LabExecutionEngine.GetLabName(labId);
            var variants = LabRepository.GetTaskVariantsByLabVarId(labVarId);
            var labWork = new LabWorkExecutionModel(labName, labId, variants);

            labWork.SetCurrent(0);
            Session["LabWork"] = labWork;
            return View(labWork);
        }

        public ActionResult ChangeTask(int Task)
        {
            LabWorkExecutionModel model = (LabWorkExecutionModel)Session["LabWork"];
            model.SetCurrent(Task);
            Session["LabWork"] = model;
            return View("Index", model);
        }
    }
}
