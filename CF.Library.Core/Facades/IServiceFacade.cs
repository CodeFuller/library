namespace CF.Library.Core.Facades
{
	/// <summary>
	/// Facade interface for ServiceBase.
	/// </summary>
	public interface IServiceFacade
	{
		/// <summary>
		/// Property Injection for Console Facade.
		/// </summary>
		IConsoleFacade ConsoleFacade { get; set; }

		/// <summary>
		/// Facade method for ServiceBase.Run(ServiceBase service).
		/// </summary>
		void RunService();

		/// <summary>
		/// Facade method for ServiceBase.OnStart(string[] args).
		/// </summary>
		void InvokeOnStart();

		/// <summary>
		/// Facade method for ServiceBase.OnStop().
		/// </summary>
		void InvokeOnStop();

		/// <summary>
		/// Runs service in interactive (debug) mode.
		/// </summary>
		void RunInteractive();

		/// <summary>
		/// Returns a string array containing the command-line arguments for the process.
		/// </summary>
		string[] GetCommandLineArgs();
	}
}
