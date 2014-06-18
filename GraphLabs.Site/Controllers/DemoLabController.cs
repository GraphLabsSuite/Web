using GraphLabs.DomainModel;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class DemoLabController : GraphLabsController
    {
        #region Зависимости

        private ILabRepository _LabsRepository
        {
            get { return DependencyResolver.GetService<ILabRepository>(); }
        }

        private ISystemDateService _DateService
        {
            get { return DependencyResolver.GetService<ISystemDateService>(); }
        }

        #endregion

        public ActionResult Index()
        {
            var model = new List<DemoLabModel>();

            foreach (var lab in _LabsRepository.GetDemoLabs(_DateService.Now()))
            {
                model.Add(new DemoLabModel(lab, _LabsRepository.GetCompleteDemoLabVariantsByLabWorkId(lab.Id)));
            }

            return View(model);
        }
    }
}
