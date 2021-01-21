using System.Collections.ObjectModel;

namespace CodeFuller.Library.Wpf.Logging
{
	public interface ILoggerViewModel
	{
		ObservableCollection<LogMessage> Messages { get; }
	}
}
