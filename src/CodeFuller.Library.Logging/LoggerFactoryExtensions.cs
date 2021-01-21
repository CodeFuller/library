using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Logging
{
	public static class LoggerFactoryExtensions
	{
		public static void LoadLoggingConfiguration(this ILoggerFactory loggerFactory, IConfiguration configuration)
		{
			LoggingSettings loggingSettings = new LoggingSettings();
			configuration.Bind("logging", loggingSettings);

			ILoggingConfiguration loggingConfiguration = new LoggingConfiguration();
			loggingConfiguration.LoadSettings(loggingSettings);
			loggingConfiguration.AddLogging(loggerFactory);
		}
	}
}
