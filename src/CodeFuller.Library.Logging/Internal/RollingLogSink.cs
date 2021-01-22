using System;
using CodeFuller.Library.Logging.Interfaces;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace CodeFuller.Library.Logging.Internal
{
	internal class RollingLogSink : ILogEventSink
	{
		private readonly ITextFormatter formatter;

		private readonly IRollingLogFile rollingLogFile;

		public RollingLogSink(ITextFormatter formatter, IRollingLogFile rollingLogFile)
		{
			this.formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
			this.rollingLogFile = rollingLogFile ?? throw new ArgumentNullException(nameof(rollingLogFile));
		}

		public void Emit(LogEvent logEvent)
		{
			_ = logEvent ?? throw new ArgumentNullException(nameof(logEvent));

			lock (formatter)
			{
				formatter.Format(logEvent, rollingLogFile.StreamWriter);
			}
		}
	}
}
