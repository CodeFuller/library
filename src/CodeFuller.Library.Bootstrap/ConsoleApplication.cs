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

		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleApplication"/> class with provided <see cref="BasicApplicationBootstrapper{TRoot}"/>.
		/// </summary>
		/// <param name="bootstrapper">Application bootstrapper.</param>
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
				logger = bootstrapper.TryGetLogger<ConsoleApplication>();

				using var cts = new CancellationTokenSource();

				Console.CancelKeyPress += (_, _) =>
				{
					logger?.LogInformation("CTRL + C is pressed");
					cts.Cancel();
				};

				return await application.Run(args, cts.Token);
			}
			catch (OperationCanceledException)
			{
				logger?.LogInformation("The program execution was aborted");
				return 1;
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
			{
#pragma warning disable CA1508 // Avoid dead conditional code
				if (logger != null)
#pragma warning restore CA1508 // Avoid dead conditional code
				{
					logger.LogCritical(e, "Exception caught");
				}
				else
				{
					await Console.Error.WriteLineAsync(e.ToString());
				}

				return e.HResult;
			}
		}
	}
}
