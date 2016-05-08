using System;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using log4net;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using log4net.Config;
using log4net.Repository;
using log4net.Util;

namespace CF.Extensions
{
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

			set { log = value; }
		}

		public static void Error(Exception exc)
		{
			Log.Error(exc);
		}
		public static void Error(string format, params object[] args)
		{
			Log.ErrorFormat(format, args);
		}
		public static void Warning(string format, params object[] args)
		{
			Log.WarnFormat(format, args);
		}
		public static void Info(string format, params object[] args)
		{
			Log.InfoFormat(format, args);
		}
		public static void Debug(string format, params object[] args)
		{
			Log.DebugFormat(format, args);
		}

		/// <remarks>
		/// This method doesn't actually work because of insufficient log level configuration (failed to configure log4net properly for dumping trace or verbose entries)
		/// </remarks>
		public static void Trace(string format, params object[] args)
		{
			Log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType, Level.Trace, String.Format(format, args), null);
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
			var logFilenamePattern = String.Format("{0}\\logs\\{1}-%date{{yyyy_MM_dd_HH_mm_ss}}.log", directoryPath, rawName);

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
