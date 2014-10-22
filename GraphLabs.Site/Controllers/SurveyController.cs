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
            result.questionOptions = new KeyValuePair<string, bool>[3];
            result.questionOptions[0] = new KeyValuePair<string, bool> ("2131231xdcfsdrevf", false);
            result.questionOptions[1] = new KeyValuePair<string, bool>("213123fsdrevf", false);
            result.questionOptions[2] = new KeyValuePair<string, bool>("sfg231xdcfsdrevf", true);
            return View(result);
        }

        [HttpPost]
        public ActionResult Create(string question, Dictionary<string, bool> questionOptions)
        {
            if (true /* тут валидация*/)
            {
                //тут сохранение через дергание репозитория
            }
            return View();
        }

    }
}
