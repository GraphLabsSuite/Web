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
			result.Question = "question";
			result.QuestionOptions.Add(new KeyValuePair<string, bool>( "xxx", true ));
            return View(result);
        }

        [HttpPost]
        public ActionResult Create(string Question, Dictionary<string, bool> QuestionOptions)
        {
			var model = new SurveyCreatingModel(Question, QuestionOptions);
            if (model.IsValid)
            {
                //тут сохранение через дергание репозитория
            }
            return View(model);
        }

    }
}
