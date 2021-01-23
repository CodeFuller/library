using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace CodeFuller.Library.Logging.Internal
{
	internal class LoggingConfiguration
	{
		private readonly LoggerConfiguration configuration;

		public LoggingConfiguration()
		{
			configuration = new LoggerConfiguration().Enrich.FromLogContext()
				.Enrich.With(new ThreadIdLogEventEnricher())
				.MinimumLevel.Verbose();
		}

		public LoggingConfiguration LoadSettings(LoggingSettings settings)
		{
			foreach (var targetConfig in settings.Targets)
			{
				var targetLogLevel = targetConfig.LogLevel ?? settings.DefaultLogLevel;
				if (String.Equals(targetConfig.Type, "Console", StringComparison.OrdinalIgnoreCase))
				{
					AddConsoleTarget(targetLogLevel);
				}
				else if (String.Equals(targetConfig.Type, "RollingFile", StringComparison.OrdinalIgnoreCase))
				{
					var targetSettings = targetConfig.Settings;
					AddRollingFileTarget(
						targetLogLevel,
						targetSettings.GetOptionalSetting<string>("logPath"),
						targetSettings.GetOptionalSetting<string>("fileNamePattern"),
						targetSettings.GetOptionalSetting<string>("firstFileNamePattern"),
						targetSettings.GetOptionalSetting<string>("fileNameExtension"),
						targetSettings.GetOptionalSetting<long>("rollSize"),
						targetSettings.GetOptionalSetting<string>("messageFormat"));
				}
				else
				{
					throw new NotSupportedException($"The log target type '{targetConfig.Type}' is not supported");
				}
			}

			return this;
		}

		private LoggingConfiguration AddConsoleTarget(LogLevel logLevel = LogLevel.Information)
		{
			configuration.WriteTo.Console(restrictedToMinimumLevel: CovertLogLevel(logLevel));
			return this;
		}

		private LoggingConfiguration AddRollingFileTarget(LogLevel logLevel, string logPath, string fileNamePattern,
			string firstFileNamePattern, string fileNameExtension, long rollSize, string messageFormat)
		{
			var entryAssemblyPath = Assembly.GetEntryAssembly()?.Location;

			string GetDefaultLogPath()
			{
				var appDirectory = Path.GetDirectoryName(entryAssemblyPath);
				return String.IsNullOrEmpty(appDirectory) ? "logs" : Path.Combine(appDirectory, "logs");
			}

			string GetDefaultFileNamePattern()
			{
				var prefix = String.IsNullOrEmpty(entryAssemblyPath) ? String.Empty : $"{Path.GetFileNameWithoutExtension(entryAssemblyPath)} - ";
				return $"{prefix}{{YYYY}}_{{MM}}_{{DD}} - {{HH}}_{{mm}}_{{SS}} - {{PID}}";
			}

			logPath ??= GetDefaultLogPath();
			fileNamePattern ??= GetDefaultFileNamePattern();
			firstFileNamePattern ??= $"{fileNamePattern} - START";
			fileNameExtension ??= "log";
			rollSize = rollSize > 0 ? rollSize : 4 * 1024 * 1024;
			messageFormat ??= "{Timestamp:yyyy.MM.dd HH:mm:ss.fff} [{Level:u3}] [TID: {PrettyThreadId}] {Message}{NewLine}{Exception}";

#pragma warning disable CA2000 // Dispose objects before losing scope
			var rollingLogFile = new RollingLogFile(logPath, firstFileNamePattern, fileNamePattern, fileNameExtension, rollSize);
#pragma warning restore CA2000 // Dispose objects before losing scope

			var formatter = new MessageTemplateTextFormatter(messageFormat, null);
			var sink = new RollingLogSink(formatter, rollingLogFile);
			configuration.WriteTo.Sink(sink, CovertLogLevel(logLevel));

			return this;
		}

		public ILoggingBuilder AddLogging(ILoggingBuilder loggingBuilder)
		{
			Log.Logger = configuration.CreateLogger();
			return loggingBuilder.AddSerilog();
		}

		public ILoggerFactory AddLogging(ILoggerFactory loggerFactory)
		{
			Log.Logger = configuration.CreateLogger();
			return loggerFactory.AddSerilog();
		}

		private static LogEventLevel CovertLogLevel(LogLevel logLevel)
		{
			return logLevel switch
			{
				LogLevel.Trace => LogEventLevel.Verbose,
				LogLevel.Debug => LogEventLevel.Debug,
				LogLevel.Information => LogEventLevel.Information,
				LogLevel.Warning => LogEventLevel.Warning,
				LogLevel.Error => LogEventLevel.Error,
				LogLevel.Critical => LogEventLevel.Fatal,
				_ => throw new NotSupportedException($"Log level '{logLevel}' is not supported"),
			};
		}
	}
}
