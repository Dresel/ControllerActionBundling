namespace ControllerActionBundling.Core
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Web.Caching;
	using System.Web.Hosting;

	public class ControllerActionVirtualPathProvider : VirtualPathProvider
	{
		public ControllerActionVirtualPathProvider(VirtualPathProvider virtualPathProvider)
		{
			// Wrap an existing virtual path provider
			VirtualPathProvider = virtualPathProvider;
		}

		protected VirtualPathProvider VirtualPathProvider { get; set; }

		public override string CombineVirtualPaths(string basePath, string relativePath)
		{
			return VirtualPathProvider.CombineVirtualPaths(basePath, relativePath);
		}

		public override bool DirectoryExists(string virtualDir)
		{
			return VirtualPathProvider.DirectoryExists(virtualDir);
		}

		public override bool FileExists(string virtualPath)
		{
			if (ControllerActionHelper.IsControllerActionRoute(virtualPath))
			{
				return true;
			}

			return VirtualPathProvider.FileExists(virtualPath);
		}

		public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies,
			DateTime utcStart)
		{
			AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();

			List<string> virtualPathDependenciesCopy = virtualPathDependencies.Cast<string>().ToList();

			// Create CacheDependencies for our virtual Controller Action paths
			foreach (string virtualPathDependency in virtualPathDependenciesCopy.ToList())
			{
				if (ControllerActionHelper.IsControllerActionRoute(virtualPathDependency))
				{
					aggregateCacheDependency.Add(new ControllerActionCacheDependency(virtualPathDependency));
					virtualPathDependenciesCopy.Remove(virtualPathDependency);
				}
			}

			// Aggregate them with the base cache dependency for virtual file paths
			aggregateCacheDependency.Add(VirtualPathProvider.GetCacheDependency(virtualPath, virtualPathDependenciesCopy,
				utcStart));

			return aggregateCacheDependency;
		}

		public override string GetCacheKey(string virtualPath)
		{
			return VirtualPathProvider.GetCacheKey(virtualPath);
		}

		public override VirtualDirectory GetDirectory(string virtualDir)
		{
			return VirtualPathProvider.GetDirectory(virtualDir);
		}

		public override VirtualFile GetFile(string virtualPath)
		{
			if (ControllerActionHelper.IsControllerActionRoute(virtualPath))
			{
				return new ControllerActionVirtualFile(virtualPath,
					new MemoryStream(Encoding.Default.GetBytes(ControllerActionHelper.RenderControllerActionToString(virtualPath))));
			}

			return VirtualPathProvider.GetFile(virtualPath);
		}

		public override string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
		{
			return VirtualPathProvider.GetFileHash(virtualPath, virtualPathDependencies);
		}

		public override object InitializeLifetimeService()
		{
			return VirtualPathProvider.InitializeLifetimeService();
		}
	}
}