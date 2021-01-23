using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Logging
{
	/// <summary>
	/// Logging target settings.
	/// </summary>
	public class LoggingTargetSettings
	{
		/// <summary>
		/// Gets or sets the type of logging target (e.g. Console, RollingFile, etc.).
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets the log level for the target. If missing, <see cref="LoggingSettings.DefaultLogLevel"/> is used.
		/// </summary>
		public LogLevel? LogLevel { get; set; }

		/// <summary>
		/// Gets target key-value settings.
		/// </summary>
		public IDictionary<string, string> Settings { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	}
}
