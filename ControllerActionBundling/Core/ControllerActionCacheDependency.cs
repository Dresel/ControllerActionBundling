namespace ControllerActionBundling.Core
{
	using System;
	using System.Threading;
	using System.Web.Caching;

	public class ControllerActionCacheDependency : CacheDependency
	{
		public ControllerActionCacheDependency(string virtualPath, int actualizationTime = 10000)
		{
			VirtualPath = virtualPath;
			LastContent = GetContentFromControllerAction();

			Timer = new Timer(CheckDependencyCallback, this, actualizationTime, actualizationTime);
		}

		private string LastContent { get; set; }

		private Timer Timer { get; set; }

		private string VirtualPath { get; set; }

		protected override void DependencyDispose()
		{
			if (Timer != null)
			{
				Timer.Dispose();
			}

			base.DependencyDispose();
		}

		private void CheckDependencyCallback(object sender)
		{
			if (Monitor.TryEnter(Timer))
			{
				try
				{
					string contentFromAction = GetContentFromControllerAction();

					if (contentFromAction != LastContent)
					{
						LastContent = contentFromAction;
						NotifyDependencyChanged(sender, EventArgs.Empty);
					}
				}
				finally
				{
					Monitor.Exit(Timer);
				}
			}
		}

		private string GetContentFromControllerAction()
		{
			return ControllerActionHelper.RenderControllerActionToString(VirtualPath);
		}
	}
}