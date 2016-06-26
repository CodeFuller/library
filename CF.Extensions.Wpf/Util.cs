using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CF.Extensions.Wpf
{
	/// <summary>
	/// Holder for util WPF methods
	/// </summary>
	public static class Util
	{
		/// <summary>
		/// Shows message box and logs it together with user choice
		/// </summary>
		public static MessageBoxResult ShowLoggedMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
		{
			var separator = "---------------------------";

			string windowLogEntry = "";
			windowLogEntry += separator + "\n";
			windowLogEntry += caption + "\n";
			windowLogEntry += separator + "\n";
			windowLogEntry += messageBoxText + "\n";
			windowLogEntry += separator + "\n";
			windowLogEntry += GetMessageBoxButtonsLogEntry(button) + "\n";
			windowLogEntry += separator + "\n";

			Logger.Info("Showing message box:\n\n{0}", windowLogEntry);
			var result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon);
			Logger.Info("User pressed [{0}]", GetMessageBoxResultLogEntry(result));

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
