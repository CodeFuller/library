using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Logging
{
	public interface ILoggingConfiguration
	{
		ILoggingConfiguration AddConsoleTarget(LogLevel logLevel = LogLevel.Information);

		ILoggingConfiguration AddRollingFileTarget(LogLevel logLevel = LogLevel.Information, string logPath = null,
			string fileNamePattern = null, string firstFileNamePattern = null, string fileNameExtension = "log",
			long rollSize = 4 * 1024 * 1024, bool jsonOutput = false,
			string messageFormat = "{Timestamp:yyyy.MM.dd HH:mm:ss.fff} [{Level:u3}] [TID: {PrettyThreadId}] {Message}{NewLine}{Exception}");

		ILoggingConfiguration LoadSettings(LoggingSettings settings);

		ILoggingBuilder AddLogging(ILoggingBuilder loggingBuilder);

		ILoggerFactory AddLogging(ILoggerFactory loggerFactory);
	}
}
