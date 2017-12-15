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
			this.bootstrapper = bootstrapper ?? throw new ArgumentNullException(nameof(bootstrapper));
		}

		/// <summary>
		/// Runs console application.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "All exceptions are caught and logged by this top-level application routine")]
		public int Run(string[] args)
		{
			try
			{
				IApplicationLogic appLogic = bootstrapper.Run(args);
				return appLogic.Run(args);
			}
			catch (Exception e)
			{
				Console.WriteLine(Current($"Exception caught: {e}"));
				return 1;
			}
		}
	}
}
