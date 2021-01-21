using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;

namespace CodeFuller.Library.Logging
{
	public class LoggingConfiguration : ILoggingConfiguration
	{
		private readonly LoggerConfiguration configuration;

		public LoggingConfiguration()
		{
			configuration = new LoggerConfiguration().Enrich.FromLogContext()
				.Enrich.With(new ThreadIdEnricher())
				.MinimumLevel.Verbose();
		}

		public ILoggingConfiguration AddConsoleTarget(LogLevel logLevel = LogLevel.Information)
		{
			configuration.WriteTo.Console(restrictedToMinimumLevel: CovertLogLevel(logLevel));
			return this;
		}

		public ILoggingConfiguration AddRollingFileTarget(LogLevel logLevel, string logPath, string fileNamePattern, string firstFileNamePattern,
			string fileNameExtension, long rollSize, bool jsonOutput, string messageFormat)
		{
			var entryAssemblyPath = Assembly.GetEntryAssembly()?.Location;
			if (logPath == null)
			{
				var appDirectory = Path.GetDirectoryName(entryAssemblyPath);
				logPath = String.IsNullOrEmpty(appDirectory) ? "logs" : Path.Combine(appDirectory, "logs");
			}

			if (fileNamePattern == null)
			{
				var prefix = String.IsNullOrEmpty(entryAssemblyPath) ? String.Empty : $"{Path.GetFileNameWithoutExtension(entryAssemblyPath)} - ";
				fileNamePattern = $"{prefix}{{YYYY}}_{{MM}}_{{DD}} - {{HH}}_{{mm}}_{{SS}} - {{PID}}";
			}

			firstFileNamePattern = firstFileNamePattern ?? $"{fileNamePattern} - START";

			var formatter = jsonOutput ? new JsonFormatter() as ITextFormatter : new MessageTemplateTextFormatter(messageFormat, null);
			var sink = new RollingFileSink(formatter, logPath, firstFileNamePattern, fileNamePattern, fileNameExtension, rollSize);
			configuration.WriteTo.Sink(sink, CovertLogLevel(logLevel));

			return this;
		}

		public ILoggingConfiguration LoadSettings(LoggingSettings settings)
		{
			foreach (TargetConfiguration targetConfig in settings.Targets)
			{
				var currTargetLogLevel = targetConfig.LogLevel ?? settings.DefaultLogLevel;
				if (String.Equals(targetConfig.Type, "Console", StringComparison.OrdinalIgnoreCase))
				{
					AddConsoleTarget(currTargetLogLevel);
				}
				else if (String.Equals(targetConfig.Type, "RollingFile", StringComparison.OrdinalIgnoreCase))
				{
					var targetSettings = targetConfig.Settings;
					AddRollingFileTarget(currTargetLogLevel,
						logPath: targetSettings.GetOptional<string>("LogPath"),
						fileNamePattern: targetSettings.GetOptional<string>("FileNamePattern"),
						firstFileNamePattern: targetSettings.GetOptional<string>("FirstFileNamePattern"),
						fileNameExtension: targetSettings.GetOptional<string>("FileNameExtension"),
						rollSize: targetSettings.GetOptional<long>("RollSize"),
						jsonOutput: targetSettings.GetOptional<bool>("JsonOutput"),
						messageFormat: targetSettings.GetOptional<string>("MessageFormat"));
				}
				else
				{
					throw new InvalidOperationException($"Unknown target {targetConfig.Type}");
				}
			}

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
			switch (logLevel)
			{
				case LogLevel.Trace:
					return LogEventLevel.Verbose;

				case LogLevel.Debug:
					return LogEventLevel.Debug;

				case LogLevel.Information:
					return LogEventLevel.Information;

				case LogLevel.Warning:
					return LogEventLevel.Warning;

				case LogLevel.Error:
					return LogEventLevel.Error;

				case LogLevel.Critical:
					return LogEventLevel.Fatal;

				default:
					throw new InvalidOperationException($"Unexpected Log level '{logLevel}'");
			}
		}
	}
}
