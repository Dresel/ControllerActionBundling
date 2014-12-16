namespace ControllerActionBundling
{
	using System.Web.Optimization;
	using ControllerActionBundling.Core;

	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			// Set the virtual path provider
			BundleTable.VirtualPathProvider = new ControllerActionVirtualPathProvider(BundleTable.VirtualPathProvider);

			BundleTable.EnableOptimizations = true;

			bundles.Add(
				new Bundle("~/scripts").Include("~/Content/static.js")
					.Include("~/JavaScript/ClassicRoute")
					.Include("~/JavaScript/AttributeRoute"));
		}
	}
}