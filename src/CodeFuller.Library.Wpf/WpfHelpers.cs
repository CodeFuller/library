using System;
using System.Windows;
using static System.FormattableString;

namespace CodeFuller.Library.Wpf
{
	/// <summary>
	/// Holder for util WPF methods
	/// </summary>
	public static class WpfHelpers
	{
		/// <summary>
		/// Shows message box and logs it together with user choice.
		/// </summary>
		public static MessageBoxResult ShowLoggedMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, Action<string> writeLog)
		{
			if (writeLog == null)
			{
				throw new ArgumentNullException(nameof(writeLog));
			}

			var separator = "---------------------------";

			string windowLogEntry = "";
			windowLogEntry += separator + "\n";
			windowLogEntry += caption + "\n";
			windowLogEntry += separator + "\n";
			windowLogEntry += messageBoxText + "\n";
			windowLogEntry += separator + "\n";
			windowLogEntry += GetMessageBoxButtonsLogEntry(button) + "\n";
			windowLogEntry += separator + "\n";

			writeLog("Showing message box:\n\n" + windowLogEntry);
			var result = MessageBox.Show(messageBoxText, caption, button, icon);
			writeLog(Invariant($"User pressed [{GetMessageBoxResultLogEntry(result)}]"));

			return result;
		}

		private static string GetMessageBoxButtonsLogEntry(MessageBoxButton buttons)
		{
			switch (buttons)
			{
				case MessageBoxButton.OK:
					return "OK";
				case MessageBoxButton.OKCancel:
					return "OK   Cancel";
				case MessageBoxButton.YesNoCancel:
					return "Yes   No   Cancel";
				case MessageBoxButton.YesNo:
					return "Yes   No";
				default:
					return buttons.ToString();
			}
		}

		private static string GetMessageBoxResultLogEntry(MessageBoxResult result)
		{
			switch (result)
			{
				case MessageBoxResult.None:
					return "None";
				case MessageBoxResult.OK:
					return "OK";
				case MessageBoxResult.Cancel:
					return "Cancel";
				case MessageBoxResult.Yes:
					return "Yes";
				case MessageBoxResult.No:
					return "No";
				default:
					return result.ToString();
			}
		}
	}
}
