using System;
using System.ServiceProcess;
using CF.Library.Core.Attributes;

namespace CF.Library.Core.Facades
{
	/// <summary>
	/// Facade for ServiceBase.
	/// </summary>
	[ExcludeFromTestCoverage("Facade class with no logic")]
	public abstract class ServiceFacade : ServiceBase, IServiceFacade
	{
		/// <summary>
		/// Property Injection for Console Facade.
		/// </summary>
		public IConsoleFacade ConsoleFacade { get; set; } = new ConsoleFacade();

		/// <summary>
		/// Implementation for ServiceBase.OnStart()
		/// </summary>
		protected abstract override void OnStart(string[] args);

		/// <summary>
		/// Implementation for ServiceBase.OnStop()
		/// </summary>
		protected abstract override void OnStop();

		/// <summary>
		/// Implementation for IServiceFacade.RunService().
		/// </summary>
		/// <remarks>
		/// Method is virtual for correct work of NSubstitute
		/// </remarks>
		[ExcludeFromTestCoverage("If ServiceBase.Run() is called not by the Service Control Manager, error message box is displayed")]
		public virtual void RunService()
		{
			ServiceBase.Run(this);
		}

		/// <summary>
		/// Implementation for IServiceFacade.InvokeOnStart().
		/// </summary>
		public void InvokeOnStart()
		{
			OnStart(GetCommandLineArgs());
		}

		/// <summary>
		/// Implementation for IServiceFacade.InvokeOnStop().
		/// </summary>
		public void InvokeOnStop()
		{
			OnStop();
		}

		/// <summary>
		/// Implementation for IServiceFacade.InvokeOnStop().
		/// </summary>
		public void RunInteractive()
		{
			InvokeOnStart();
			ConsoleFacade.WriteLine(@"Press any key to stop execution");
			ConsoleFacade.Read();
			InvokeOnStop();
		}

		/// <summary>
		/// Implementation for IServiceFacade.GetCommandLineArgs()
		/// </summary>
		/// <remarks>
		/// Method is virtual for correct work of NSubstitute
		/// </remarks>
		public virtual string[] GetCommandLineArgs()
		{
			return Environment.GetCommandLineArgs();
		}
	}
}
