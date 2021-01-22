namespace CodeFuller.Library.Wpf.Interfaces
{
	/// <summary>
	/// Interface for window service.
	/// </summary>
	public interface IWindowService
	{
		/// <summary>
		/// Displays a message box with specified message.
		/// </summary>
		ShowMessageBoxResult ShowMessageBox(string message);

		/// <summary>
		/// Displays a message box with specified message, title, buttons and icon.
		/// </summary>
		ShowMessageBoxResult ShowMessageBox(string message, string title, ShowMessageBoxButton button, ShowMessageBoxIcon icon);
	}
}
