using System;
using System.Threading;
using static CF.Library.Core.Extensions.FormattableStringExtensions;

namespace CF.Library.Core.Logging
{
	public class RollingFileLogger : IMessageLogger
	{
		private readonly IFileLogger fileLogger;

		public RollingFileLogger(IFileLogger fileLogger)
		{
			this.fileLogger = fileLogger ?? throw new ArgumentNullException(nameof(fileLogger));
		}

		public void WriteDebug(string message)
		{
			Write("DEBUG", message);
		}

		public void WriteInfo(string message)
		{
			Write("INFO", message);
		}

		public void WriteWarning(string message)
		{
			Write("WARNING", message);
		}

		public void WriteError(string message)
		{
			Write("ERROR", message);
		}

		private void Write(string level, string message)
		{
			var formattedMessage = Current($"{DateTime.Now:yyyy.MM.dd  HH:mm:ss.fff}  [TID: {Thread.CurrentThread.ManagedThreadId, 3}]  {level + ":",-8}    {message}\n");
			fileLogger.Write(formattedMessage);
		}
	}
}
