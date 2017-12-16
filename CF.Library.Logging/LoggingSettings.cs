using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CF.Library.Logging
{
	public class LoggingSettings
	{
		public LogLevel DefaultLogLevel { get; set; }

		public List<TargetConfiguration> Targets { get; set; }
	}
}
