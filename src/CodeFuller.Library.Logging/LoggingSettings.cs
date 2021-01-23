using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Logging
{
	/// <summary>
	/// Application logging settings.
	/// </summary>
	public class LoggingSettings
	{
		/// <summary>
		/// Gets or sets default log level, which is used for the logging target if its log level is not specified.
		/// </summary>
		public LogLevel DefaultLogLevel { get; set; }

		/// <summary>
		/// Gets collection of logging targets settings.
		/// </summary>
		public ICollection<LoggingTargetSettings> Targets { get; } = new Collection<LoggingTargetSettings>();
	}
}
