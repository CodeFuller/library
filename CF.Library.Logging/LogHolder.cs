using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CF.Library.Logging
{
	public static class LogHolder
	{
		public static ILogger Logger { get; set; } = NullLogger.Instance;
	}
}
