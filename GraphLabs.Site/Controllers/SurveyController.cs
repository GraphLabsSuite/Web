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
            result.questionOptions = new List<KeyValuePair<string, bool>>();
            result.questionOptions.Add(new KeyValuePair<string, bool> ("2131231xdcfsdrevf", false));
            return View(result);
        }


    }
}
