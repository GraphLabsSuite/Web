using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Controllers.LabWorks;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class SurveyController : GraphLabsController
    {

        public ActionResult Create ()
        {
            var result = new SurveyCreatingModel();
            result.QuestionOptions = new QuestionOptions[3];
            result.Question = "question";
            result.QuestionOptions[0] = new QuestionOptions("answer1", true);
            result.QuestionOptions[1] = new QuestionOptions("answer2", false);
            result.QuestionOptions[2] = new QuestionOptions("answer3", false);
            return View(result);
        }

        [HttpPost]
        public ActionResult Create(SurveyCreatingModel model)
        {
            if (true /* тут валидация*/)
            {
                //тут сохранение через дергание репозитория
            }
            return View();
        }

    }
}
