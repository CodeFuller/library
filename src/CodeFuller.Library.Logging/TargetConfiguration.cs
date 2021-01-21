using Microsoft.Extensions.Logging;

namespace CodeFuller.Library.Logging
{
	public class TargetConfiguration
	{
		public string Type { get; set; }

		public LogLevel? LogLevel { get; set; }

		public TargetSettings Settings { get; set; }
	}
}
