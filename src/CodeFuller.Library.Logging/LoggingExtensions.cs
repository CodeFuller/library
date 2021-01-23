using System;
using CodeFuller.Library.Logging.Internal;
using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Logging
{
	/// <summary>
	/// Extension methods for setting up logging pipeline.
	/// </summary>
	public static class LoggingExtensions
	{
		/// <summary>
		/// Loads logging settings from provided configuration and sets up logging pipeline.
		/// </summary>
		/// <param name="loggerFactory">The logger factory to configure.</param>
		/// <param name="setupSettings">The action for filling <see cref="LoggingSettings"/>.</param>
		/// <returns>The <see cref="ILoggerFactory"/> so that additional calls can be chained.</returns>
		public static ILoggerFactory AddLogging(this ILoggerFactory loggerFactory, Action<LoggingSettings> setupSettings)
		{
			return AddLogging(setupSettings, loggingConfiguration => loggingConfiguration.AddLogging(loggerFactory));
		}

		/// <summary>
		/// Loads logging settings from provided configuration and sets up logging pipeline.
		/// </summary>
		/// <param name="loggingBuilder">The logging builder to configure.</param>
		/// <param name="setupSettings">The action for filling <see cref="LoggingSettings"/>.</param>
		/// <returns>The <see cref="ILoggingBuilder"/> so that additional calls can be chained.</returns>
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
