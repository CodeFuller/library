﻿using System;
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
			foreach (TargetConfiguration targetConfig in settings.Targets)
			{
				var targetLogLevel = targetConfig.LogLevel ?? settings.DefaultLogLevel;
				if (String.Equals(targetConfig.Type, "Console", StringComparison.OrdinalIgnoreCase))
				{
					AddConsoleTarget(targetLogLevel);
				}
				else if (String.Equals(targetConfig.Type, "RollingFile", StringComparison.OrdinalIgnoreCase))
				{
					var targetSettings = targetConfig.Settings;
					AddRollingFileTarget(targetLogLevel,
						logPath: targetSettings.GetOptionalSetting<string>("logPath"),
						fileNamePattern: targetSettings.GetOptionalSetting<string>("fileNamePattern"),
						firstFileNamePattern: targetSettings.GetOptionalSetting<string>("firstFileNamePattern"),
						fileNameExtension: targetSettings.GetOptionalSetting<string>("fileNameExtension"),
						rollSize: targetSettings.GetOptionalSetting<long>("rollSize"),
						messageFormat: targetSettings.GetOptionalSetting<string>("MessageFormat"));
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

			var rollingLogFile = new RollingLogFile(logPath, firstFileNamePattern, fileNamePattern, fileNameExtension, rollSize);
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
					throw new NotSupportedException($"Log level '{logLevel}' is not supported");
			}
		}
	}
}