using System.Threading;
using Serilog.Core;
using Serilog.Events;

namespace CodeFuller.Library.Logging.Internal
{
	internal class ThreadIdLogEventEnricher : ILogEventEnricher
	{
		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ThreadId", Thread.CurrentThread.ManagedThreadId));
			logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("PrettyThreadId", $"{Thread.CurrentThread.ManagedThreadId,3}"));
		}
	}
}
