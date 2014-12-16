namespace ControllerActionBundling.Core
{
	using System.IO;
	using System.Web.Hosting;

	public class ControllerActionVirtualFile : VirtualFile
	{
		public ControllerActionVirtualFile(string virtualPath, Stream stream)
			: base(virtualPath)
		{
			Stream = stream;
		}

		public Stream Stream { get; private set; }

		public override Stream Open()
		{
			return Stream;
		}
	}
}