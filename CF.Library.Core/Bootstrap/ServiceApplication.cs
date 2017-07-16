using System;
using System.Linq;
using CF.Library.Core.Facades;

namespace CF.Library.Core.Bootstrap
{
	/// <summary>
	/// Implementation for IServiceApplication.
	/// </summary>
	public class ServiceApplication : IServiceApplication
	{
		private readonly IBootstrapper<IServiceFacade> bootstrapper;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ServiceApplication(IBootstrapper<IServiceFacade> bootstrapper)
		{
			if (bootstrapper == null)
			{
				throw new ArgumentNullException(nameof(bootstrapper));
			}

			this.bootstrapper = bootstrapper;
		}

		/// <summary>
		/// Implementation for IServiceApplication.Run().
		/// </summary>
		public void Run()
		{
			IServiceFacade service = bootstrapper.Run();
			string[] commandLineArgs = service.GetCommandLineArgs();

			bool interactiveMode = commandLineArgs.Contains("--interactive");
			if (interactiveMode)
			{
				service.RunInteractive();
			}
			else
			{
				service.RunService();
			}

			bootstrapper.Dispose();
		}
	}
}
