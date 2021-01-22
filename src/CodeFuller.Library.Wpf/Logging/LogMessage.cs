﻿using System;
using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Wpf.Logging
{
	public class LogMessage
	{
		public LogLevel Level { get; set; }

		public string Message { get; set; }

		public LogMessage(LogLevel level, string message)
		{
			Level = level;
			Message = $"{DateTime.Now:yyyy-MM-dd  HH:mm:ss}  {GetLevelString(level)}  {message}";
		}

		private static string GetLevelString(LogLevel level)
		{
			switch (level)
			{
				case LogLevel.Critical:
					return "CRITICAL:";

				case LogLevel.Error:
					return "ERROR:  ";

				case LogLevel.Warning:
					return "WARNING:";

				case LogLevel.Information:
					return "INFO:   ";

				case LogLevel.Debug:
					return "DEBUG:  ";

				case LogLevel.Trace:
					return "TRACE:  ";

				default:
					throw new NotSupportedException($"The log level {level} is not supported");
			}
		}
	}
}
