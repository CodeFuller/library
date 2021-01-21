using System;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace CodeFuller.Library.Logging
{
	public class RollingFileSink : ILogEventSink
	{
		private readonly object lockRoot = new object();

		private readonly ITextFormatter formatter;

		/// <summary>
		/// Property injection for IRollingLogFile.
		/// </summary>
		internal IRollingLogFile RollingLogFile { get; }

		public RollingFileSink(ITextFormatter formatter, string logPath, string firstFileNamePattern, string nextFileNamePattern, string fileNameExtension, long rollSize)
		{
			this.formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

			RollingLogFile = new RollingLogFile(logPath, firstFileNamePattern, nextFileNamePattern, fileNameExtension, rollSize);
		}

		public void Emit(LogEvent logEvent)
		{
			if (logEvent == null)
			{
				throw new ArgumentNullException(nameof(logEvent));
			}

			lock (lockRoot)
			{
				formatter.Format(logEvent, RollingLogFile.StreamWriter);
			}
		}
	}
}
