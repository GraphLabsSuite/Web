using GraphLabs.Site.Controllers.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef.Services;
using GraphLabs.Site.Models.DemoLab;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Controllers
{
    [GLAuthorize(UserRole.Administrator, UserRole.Teacher, UserRole.Student)]
    public class DemoLabController : GraphLabsController
    {

        #region Зависимости

        private readonly IEntityQuery _query;

        private readonly ISystemDateService _dateService;

        #endregion

        public DemoLabController(
            IEntityQuery query,
            ISystemDateService dateService)
        {
            _query = query;
            _dateService = dateService;
        }

        public ActionResult Index()
        {
            var model = new List<DemoLabModel>();

            foreach (var lab in GetDemoLabs())
            {
                model.Add(new DemoLabModel(lab, GetCompleteDemoLabVariantsByLabWorkId(lab.Id)));
            }

            return View(model);
        }


        //TODO: Придумать что-то с LabModel
        /// <summary> Получить лабораторные работы, доступные в данный момент в ознакомительном режиме </summary>
        private LabWork[] GetDemoLabs()
        {
            var currentTime = _dateService.Now();
            return _query.OfEntities<LabWork>()
                .ToArray()
                .Where(l => l.AcquaintanceFrom.HasValue && l.AcquaintanceTill.HasValue)
                .Where(l => currentTime.CompareTo(l.AcquaintanceFrom) >= 0 && currentTime.CompareTo(l.AcquaintanceTill) <= 0)
                .ToArray();
        }

        /// <summary> Получить ознакомительные варианты лабораторной работы по id лабораторной работы </summary>
        private LabVariant[] GetCompleteDemoLabVariantsByLabWorkId(long id)
        {
            return _query.OfEntities<LabVariant>()
                .Where(lv => lv.LabWork.Id == id)
                .Where(lv => lv.IntroducingVariant)
                .ToArray()
                .Where(lv => VerifyCompleteVariant(lv.Id))
                .ToArray();
        }

        /// <summary> Проверка соответствия варианта лабораторной работы содержанию работы </summary>
        public bool VerifyCompleteVariant(long variantId)
        {
            long labWorkId = _query.OfEntities<LabVariant>()
                .Where(v => v.Id == variantId)
                .Select(v => v.LabWork.Id)
                .Single();

            long[] labEntry = _query.OfEntities<LabEntry>()
                .Where(e => e.LabWork.Id == labWorkId)
                .Select(e => e.Task.Id)
                .ToArray();

            long[] currentVariantEntry = _query.OfEntities<LabVariant>()
                .Where(l => l.Id == variantId)
                .SelectMany(t => t.TaskVariants)
                .Select(t => t.Task.Id)
                .ToArray();

            return labEntry.ContainsSameSet(currentVariantEntry);
        }
    }
}
