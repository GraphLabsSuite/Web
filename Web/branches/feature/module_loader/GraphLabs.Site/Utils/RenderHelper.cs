﻿using System.IO;
using System.Web.Mvc;

namespace GraphLabs.Site
{
    /// <summary> Класс для рендеринга частичного представления </summary>
	public static class RenderHelper
	{
		public static string PartialView(Controller controller, string viewName, object model)
		{
			controller.ViewData.Model = model;

			using (var sw = new StringWriter())
			{
				var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
				var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);

				viewResult.View.Render(viewContext, sw);
				viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

				return sw.ToString();
			}
		}
	}
}