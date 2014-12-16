namespace ControllerActionBundling.Core
{
	using System;
	using System.IO;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;

	public static class ControllerActionHelper
	{
		public static bool IsControllerActionRoute(string virtualPath)
		{
			// With classic route configuration "{controller}/{action}/{id}" initiating a controller would work
			// when Bundling checks for .debug or .min files and this method would therefore return true
			// So check for .debug / .min manually and return false
			// If you use attribute routes only you can skip this check
			if (virtualPath.EndsWith(".debug") || virtualPath.EndsWith(".min"))
			{
				return false;
			}

			// If we can initiate a Controller instance we assume it is an Controller / Action route
			HttpContext httpContext = CreateHttpContext(virtualPath);
			HttpContextWrapper httpContextWrapper = new HttpContextWrapper(httpContext);

			RouteData routeData = RouteTable.Routes.GetRouteData(httpContextWrapper);

			if (routeData == null)
			{
				return false;
			}

			RequestContext httpResponse = new RequestContext() { HttpContext = httpContextWrapper, RouteData = routeData };

			IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();

			try
			{
				IController controller = controllerFactory.CreateController(httpResponse,
					httpResponse.RouteData.GetRequiredString("controller"));
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public static string RenderControllerActionToString(string virtualPath)
		{
			HttpContext httpContext = CreateHttpContext(virtualPath);
			HttpContextWrapper httpContextWrapper = new HttpContextWrapper(httpContext);

			RequestContext httpResponse = new RequestContext()
			{
				HttpContext = httpContextWrapper,
				RouteData = RouteTable.Routes.GetRouteData(httpContextWrapper)
			};

			// Set HttpContext.Current if RenderActionToString is called outside of a request
			if (HttpContext.Current == null)
			{
				HttpContext.Current = httpContext;
			}

			IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();
			IController controller = controllerFactory.CreateController(httpResponse,
				httpResponse.RouteData.GetRequiredString("controller"));
			controller.Execute(httpResponse);

			return httpResponse.HttpContext.Response.Output.ToString();
		}

		private static HttpContext CreateHttpContext(string virtualPath)
		{
			HttpRequest httpRequest = new HttpRequest(string.Empty, ToDummyAbsoluteUrl(virtualPath), string.Empty);
			HttpResponse httpResponse = new HttpResponse(new StringWriter());

			return new HttpContext(httpRequest, httpResponse);
		}

		private static string ToDummyAbsoluteUrl(string virtualPath)
		{
			return string.Format("http://dummy.net{0}", VirtualPathUtility.ToAbsolute(virtualPath));
		}
	}
}