using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models.Infrastructure;
using GraphLabs.Site.Models.Lab;
using GraphLabs.Site.Models.TestPool;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher)]
    public class LabController : GraphLabsController
    {
        private readonly IListModelLoader _listModelLoader;
//        private readonly IEntityBasedModelSaver<LabModel, LabWork> _modelSaver;
//        private readonly IEntityBasedModelLoader<LabModel, LabWork> _modelLoader;
//        private readonly IEntityRemover<LabWork> _modelRemover;

        public LabController(
            IListModelLoader listModelLoader
//            IEntityBasedModelSaver<LabModel, LabWork> modelSaver,
//            IEntityBasedModelLoader<LabModel, LabWork> modelLoader,
//            IEntityRemover<LabWork> modelRemover
        )
        {
            _listModelLoader = listModelLoader;
//            _modelSaver = modelSaver;
//            _modelLoader = modelLoader;
//            _modelRemover = modelRemover;
        }

        public ActionResult Index()
        {
            var model = _listModelLoader.LoadListModel<LabListModel, LabModel>();
            return View(model);
        }

    }
}
