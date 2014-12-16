namespace ControllerActionBundling.Core
{
	using System.Web.Mvc;

	public static class ControllerExtension
	{
		public static DynamicContentViewResult CascadingStyleSheetView(this Controller controller, object model)
		{
			return controller.CascadingStyleSheetView(null, null, model);
		}

		public static DynamicContentViewResult CascadingStyleSheetView(this Controller controller, string viewName)
		{
			return controller.CascadingStyleSheetView(viewName, null, null);
		}

		public static DynamicContentViewResult CascadingStyleSheetView(this Controller controller, string viewName, string masterName)
		{
			return controller.CascadingStyleSheetView(viewName, masterName, null);
		}

		public static DynamicContentViewResult CascadingStyleSheetView(this Controller controller, string viewName, object model)
		{
			return controller.CascadingStyleSheetView(viewName, null, model);
		}

		public static DynamicContentViewResult CascadingStyleSheetView(this Controller controller, string viewName, string masterName,
			object model)
		{
			if (model != null)
			{
				controller.ViewData.Model = model;
			}

			return new DynamicContentViewResult
			{
				ViewName = viewName,
				MasterName = masterName,
				ViewData = controller.ViewData,
				TempData = controller.TempData,
				ViewEngineCollection = controller.ViewEngineCollection,
				ContentType = "text/css",
				TagName = "style",
				StripTags = true
			};
		}

		public static DynamicContentViewResult CascadingStyleSheetView(this Controller controller)
		{
			return controller.CascadingStyleSheetView(null, null, null);
		}

		public static DynamicContentViewResult JavaScriptView(this Controller controller)
		{
			return controller.JavaScriptView(null, null, null);
		}

		public static DynamicContentViewResult JavaScriptView(this Controller controller, object model)
		{
			return controller.JavaScriptView(null, null, model);
		}

		public static DynamicContentViewResult JavaScriptView(this Controller controller, string viewName)
		{
			return controller.JavaScriptView(viewName, null, null);
		}

		public static DynamicContentViewResult JavaScriptView(this Controller controller, string viewName, string masterName)
		{
			return controller.JavaScriptView(viewName, masterName, null);
		}

		public static DynamicContentViewResult JavaScriptView(this Controller controller, string viewName, object model)
		{
			return controller.JavaScriptView(viewName, null, model);
		}

		public static DynamicContentViewResult JavaScriptView(this Controller controller, string viewName, string masterName, object model)
		{
			if (model != null)
			{
				controller.ViewData.Model = model;
			}

			return new DynamicContentViewResult
			{
				ViewName = viewName,
				MasterName = masterName,
				ViewData = controller.ViewData,
				TempData = controller.TempData,
				ViewEngineCollection = controller.ViewEngineCollection,
				ContentType = "text/javascript",
				TagName = "script",
				StripTags = true
			};
		}
	}
}