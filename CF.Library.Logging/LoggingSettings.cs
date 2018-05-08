using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;

namespace CF.Library.Logging
{
	public class LoggingSettings
	{
		public LogLevel DefaultLogLevel { get; set; }

		public IReadOnlyCollection<TargetConfiguration> Targets { get; set; } = new Collection<TargetConfiguration>();
	}
}
