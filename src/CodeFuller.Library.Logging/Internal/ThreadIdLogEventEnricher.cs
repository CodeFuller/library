using System;
using Serilog.Core;
using Serilog.Events;

namespace CodeFuller.Library.Logging.Internal
{
	internal class ThreadIdLogEventEnricher : ILogEventEnricher
	{
		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			var treadId = Environment.CurrentManagedThreadId;

			logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ThreadId", treadId));
			logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("PrettyThreadId", $"{treadId,3}"));
		}
	}
}
