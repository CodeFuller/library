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
		/// <param name="message">A message text to display.</param>
		/// <returns>A <see cref="ShowMessageBoxResult"/> value that specifies which message box button is clicked by the user.</returns>
		ShowMessageBoxResult ShowMessageBox(string message);

		/// <summary>
		/// Displays a message box with specified message, title, buttons and icon.
		/// </summary>
		/// <param name="message">A message text to display.</param>
		/// <param name="title">A message title to display.</param>
		/// <param name="button">A <see cref="ShowMessageBoxButton"/> value that specifies which button or buttons to display.</param>
		/// <param name="icon">A <see cref="ShowMessageBoxIcon"/> value that specifies the icon to display.</param>
		/// <returns>A <see cref="ShowMessageBoxResult"/> value that specifies which message box button is clicked by the user.</returns>
		ShowMessageBoxResult ShowMessageBox(string message, string title, ShowMessageBoxButton button, ShowMessageBoxIcon icon);
	}
}
