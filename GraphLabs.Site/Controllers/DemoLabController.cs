using GraphLabs.Dal.Ef;
using GraphLabs.Site.Controllers.Attributes;
using GraphLabs.Site.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class DemoLabController : GraphLabsController
    {

        #region Зависимости

        private readonly ILabRepository _labsRepository;

        private readonly ISystemDateService _dateService;

        #endregion

        public DemoLabController(
            ILabRepository labsRepository,
            ISystemDateService dateService)
        {
            _labsRepository = labsRepository;
            _dateService = dateService;
        }

        public ActionResult Index()
        {
            var model = new List<DemoLabModel>();

            foreach (var lab in _labsRepository.GetDemoLabs(_dateService.Now()))
            {
                model.Add(new DemoLabModel(lab, _labsRepository.GetCompleteDemoLabVariantsByLabWorkId(lab.Id)));
            }

            return View(model);
        }
    }
}
