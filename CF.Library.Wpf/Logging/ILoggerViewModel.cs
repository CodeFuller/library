using System.Collections.ObjectModel;

namespace CF.Library.Wpf.Logging
{
	public interface ILoggerViewModel
	{
		ObservableCollection<LogMessage> Messages { get; }
	}
}
