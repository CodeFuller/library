using System;
using static CF.Library.Core.Extensions.FormattableStringExtensions;

namespace CF.Library.Core.Bootstrap
{
	/// <summary>
	/// Basic implementation for console application.
	/// </summary>
	public class ConsoleApplication : IConsoleApplication
	{
		private readonly IBootstrapper<IApplicationLogic> bootstrapper;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ConsoleApplication(IBootstrapper<IApplicationLogic> bootstrapper)
		{
			if (bootstrapper == null)
			{
				throw new ArgumentNullException(nameof(bootstrapper));
			}

			this.bootstrapper = bootstrapper;
		}

		/// <summary>
		/// Runs console application.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "All exceptions are caught and logged by this top-level application routine")]
		public void Run(string[] args)
		{
			try
			{
				IApplicationLogic appLogic = bootstrapper.Run();
				appLogic.Run(args);
			}
			catch (Exception e)
			{
				Console.WriteLine(Current($"Exception caught: {e}"));
			}
		}
	}
}
