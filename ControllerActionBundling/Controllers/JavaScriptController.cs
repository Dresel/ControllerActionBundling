namespace ControllerActionBundling.Controllers
{
	using System;
	using System.Globalization;
	using System.Web.Mvc;
	using ControllerActionBundling.Core;
	using ControllerActionBundling.Models.JavaScript;

	[RoutePrefix("JavaScript")]
	public class JavaScriptController : Controller
	{
		[Route("AttributeRoute")]
		public ActionResult AttributeRoute()
		{
			// Return a dynamic (changes every minute) JavaScript View
			return this.JavaScriptView(new DynamicJavaScriptViewModel() { DateTime = DateTime.Now.ToString("g", CultureInfo.InvariantCulture) });
		}

		public ActionResult ClassicRoute()
		{
			// Return a dynamic (changes every minute) JavaScript View
			return this.JavaScriptView(new DynamicJavaScriptViewModel() { DateTime = DateTime.Now.ToString("g", CultureInfo.InvariantCulture) });
		}
	}
}