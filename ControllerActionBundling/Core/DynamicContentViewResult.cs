namespace ControllerActionBundling.Core
{
	using System;
	using System.IO;
	using System.Text.RegularExpressions;
	using System.Web.Mvc;

	// Inspired by http://blog.pmunin.com/2013/04/dynamic-javascript-css-in-aspnet-mvc.html
	public class DynamicContentViewResult : ViewResult
	{
		public DynamicContentViewResult()
		{
			StripTags = false;
		}

		public string ContentType { get; set; }

		public bool StripTags { get; set; }

		public string TagName { get; set; }

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			if (string.IsNullOrEmpty(ViewName))
			{
				ViewName = context.RouteData.GetRequiredString("action");
			}

			ViewEngineResult result = null;

			if (View == null)
			{
				result = FindView(context);
				View = result.View;
			}

			string viewResult;

			using (StringWriter viewContentWriter = new StringWriter())
			{
				ViewContext viewContext = new ViewContext(context, View, ViewData, TempData, viewContentWriter);

				View.Render(viewContext, viewContentWriter);

				if (result != null)
				{
					result.ViewEngine.ReleaseView(context, View);
				}

				viewResult = viewContentWriter.ToString();

				// Strip Tags
				if (StripTags)
				{
					string regex = string.Format("<{0}[^>]*>(.*?)</{0}>", TagName);
					Match res = Regex.Match(viewResult, regex,
						RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.Singleline);

					if (res.Success && res.Groups.Count > 1)
					{
						viewResult = res.Groups[1].Value;
					}
					else
					{
						throw new InvalidProgramException(
							string.Format("Dynamic content produced by View '{0}' expected to be wrapped in '{1}' tag.", ViewName, TagName));
					}
				}
			}

			context.HttpContext.Response.ContentType = ContentType;
			context.HttpContext.Response.Output.Write(viewResult);
		}
	}
}