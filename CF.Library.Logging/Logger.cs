using System;
using System.Globalization;
using System.IO;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Util;

namespace CF.Library.Logging
{
	/// <summary>
	/// Provides convenient way of writing log messages.
	/// It doesn't require any setup calls and could be used out of the box.
	/// </summary>
	public static class Logger
	{
		private static ILog log;

		private static ILog Log
		{
			get
			{
				if (log == null)
				{
					InitializeLog();
				}

				return log;
			}
		}

		/// <summary>
		/// Logs error message for given exception.
		/// </summary>
		public static void Error(Exception exc)
		{
			Log.Error(exc);
		}
		/// <summary>
		/// Logs error message.
		/// </summary>
		public static void Error(string format, params object[] args)
		{
			Log.ErrorFormat(CultureInfo.InvariantCulture, format, args);
		}
		/// <summary>
		/// Logs warning message.
		/// </summary>
		public static void Warning(string format, params object[] args)
		{
			Log.WarnFormat(CultureInfo.InvariantCulture, format, args);
		}
		/// <summary>
		/// Logs info message.
		/// </summary>
		public static void Info(string format, params object[] args)
		{
			Log.InfoFormat(CultureInfo.InvariantCulture, format, args);
		}
		/// <summary>
		/// Logs debug message.
		/// </summary>
		public static void Debug(string format, params object[] args)
		{
			Log.DebugFormat(CultureInfo.InvariantCulture, format, args);
		}

		/// <summary>
		/// Logs trace message.
		/// </summary>
		public static void Trace(string format, params object[] args)
		{
			Log.DebugFormat(CultureInfo.InvariantCulture, format, args);
		}

		private static void InitializeLog()
		{
			log = CreateBasicFileAppenderLog();
		}

		private static ILog CreateBasicFileAppenderLog()
		{
			var appender = new RollingFileAppender();

			appender.Name = "CF.Logger.TheRollingFileAppender";

			//  Building default log filename
			var exeFilename = System.Reflection.Assembly.GetEntryAssembly().Location;
			var directoryPath = Path.GetDirectoryName(exeFilename);
			var rawName = Path.GetFileNameWithoutExtension(exeFilename);
			var logFilenamePattern = FormattableString.Invariant($"{directoryPath}\\logs\\{rawName}-%date{{yyyy_MM_dd_HH_mm_ss}}.log");

			appender.File = (new PatternString(logFilenamePattern)).Format();
			appender.AppendToFile = false;
			appender.CountDirection = 1;
			appender.Encoding = Encoding.UTF8;
			appender.ImmediateFlush = true;
			var layout = new PatternLayout()
			{
				ConversionPattern = "%date{yyyy.MM.dd HH:mm:ss.fff}  [TID:%4thread]  %-5level  %message%newline"
			};
			layout.ActivateOptions();
			appender.Layout = layout;
			appender.MaximumFileSize = "256MB";
			appender.MaxSizeRollBackups = -1;
			appender.PreserveLogFileNameExtension = true;
			appender.RollingStyle = RollingFileAppender.RollingMode.Size;
			appender.StaticLogFileName = false;
			appender.Threshold = Level.All;


			appender.ActivateOptions();

			BasicConfigurator.Configure(appender);

			return LogManager.GetLogger("CF.Logger");
		}
	}
}
