using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Controllers
{
    public class LabController : Controller
    {

        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        //
        // GET: /Lab/

        public ActionResult Index()
        {
            this.AllowAnonymous(_ctx);
            return View();
        }

        public ActionResult CreateLabVariant(CreateLabVariant clv)
        {
            this.AllowAnonymous(_ctx);

            GetLabVariants(clv);
            
            return View("CreateLabVariant", clv);
        }

        private void GetLabVariants(CreateLabVariant clv)
        {
            Dictionary<string, List<KeyValuePair<int, string>>> x = new Dictionary<string, List<KeyValuePair<int, string>>>();
            x.Add("задание 1", new List<KeyValuePair<int, string>> { new KeyValuePair<int, string>(1, "вариант 1"), new KeyValuePair<int, string>(2, "вариант 2"), new KeyValuePair<int, string>(3, "вариант 3") });
            x.Add("задание 2", new List<KeyValuePair<int, string>> { new KeyValuePair<int, string>(1, "вариант 1") });
            x.Add("задание 3", new List<KeyValuePair<int, string>> { new KeyValuePair<int, string>(1, "вариант 1"), new KeyValuePair<int, string>(2, "вариант 2") });
            x.Add("задание 4", new List<KeyValuePair<int, string>> { new KeyValuePair<int, string>(1, "вариант 1"), new KeyValuePair<int, string>(2, "вариант 2"), new KeyValuePair<int, string>(3, "вариант 3") });
            clv.Variants = x;
        }

        public ActionResult AddLab(string name)
        {
            this.AllowAnonymous(_ctx);

            if ( String.IsNullOrEmpty(name) )
            {
                ViewBag.Error = "Отсутствует название лабораторной работы!";
                return View("Index");
            }
            //TODO: проверка существования в БД, добавление в БД
            CreateLabVariant clv = new CreateLabVariant { LabName=name };
            return RedirectToAction("CreateLabVariant", clv);
        }
    }
}
