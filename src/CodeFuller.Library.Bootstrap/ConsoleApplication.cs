using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Bootstrap
{
	/// <summary>
	/// Skeleton for console application.
	/// </summary>
	public class ConsoleApplication
	{
		private readonly BasicApplicationBootstrapper<IApplicationLogic> bootstrapper;

		public ConsoleApplication(BasicApplicationBootstrapper<IApplicationLogic> bootstrapper)
		{
			this.bootstrapper = bootstrapper ?? throw new ArgumentNullException(nameof(bootstrapper));
		}

		/// <summary>
		/// Executes console application.
		/// </summary>
		/// <param name="args">Command line arguments.</param>
		/// <returns>Exit code for the process.</returns>
		public async Task<int> Run(string[] args)
		{
			ILogger<ConsoleApplication> logger = null;

			try
			{
				var application = bootstrapper.Bootstrap(args);
				logger = bootstrapper.GetLogger<ConsoleApplication>();

				var cts = new CancellationTokenSource();

				Console.CancelKeyPress += delegate
				{
					logger.LogInformation("CTRL + C is pressed");
					cts.Cancel();
				};

				return await application.Run(args, cts.Token);
			}
			catch (OperationCanceledException)
			{
				logger?.LogInformation("The program execution was aborted");
				return 1;
			}
			catch (Exception e)
			{
				if (logger != null)
				{
					logger.LogCritical(e, "Exception caught");
				}
				else
				{
					Console.Error.WriteLine(e);
				}

				return e.HResult;
			}
		}
	}
}
