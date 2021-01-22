using System;
using CodeFuller.Library.Logging.Internal;
using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Logging
{
	public static class LoggingExtensions
	{
		/// <summary>
		/// Loads logging settings from provided configuration and sets up logging pipeline.
		/// </summary>
		public static ILoggerFactory AddLogging(this ILoggerFactory loggerFactory, Action<LoggingSettings> setupSettings)
		{
			return AddLogging(setupSettings, loggingConfiguration => loggingConfiguration.AddLogging(loggerFactory));
		}

		/// <summary>
		/// Loads logging settings from provided configuration and sets up logging pipeline.
		/// </summary>
		public static ILoggingBuilder AddLogging(this ILoggingBuilder loggingBuilder, Action<LoggingSettings> setupSettings)
		{
			return AddLogging(setupSettings, loggingConfiguration => loggingConfiguration.AddLogging(loggingBuilder));
		}

		private static T AddLogging<T>(Action<LoggingSettings> setupSettings, Func<LoggingConfiguration, T> addLogging)
		{
			var loggingSettings = new LoggingSettings();
			setupSettings(loggingSettings);

			var loggingConfiguration = new LoggingConfiguration();
			loggingConfiguration.LoadSettings(loggingSettings);

			return addLogging(loggingConfiguration);
		}
	}
}
