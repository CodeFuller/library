using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Logging
{
	public class LoggingSettings
	{
		public LogLevel DefaultLogLevel { get; set; }

		public ICollection<TargetConfiguration> Targets { get; } = new Collection<TargetConfiguration>();
	}
}
