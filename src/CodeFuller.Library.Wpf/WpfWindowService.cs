using System;
using System.Windows;
using CodeFuller.Library.Wpf.Interfaces;

namespace CodeFuller.Library.Wpf
{
	/// <summary>
	/// Implementation of IWindowService for WPF.
	/// </summary>
	public class WpfWindowService : IWindowService
	{
		/// <summary>
		/// Displays a message box with specified message, buttons and icon.
		/// </summary>
		public ShowMessageBoxResult ShowMessageBox(string message)
		{
			MessageBoxResult result = MessageBox.Show(message);
			return ConvertShowMessageBoxResult(result);
		}

		/// <summary>
		/// Displays a message box with specified message, title, buttons and icon.
		/// </summary>
		public ShowMessageBoxResult ShowMessageBox(string message, string title, ShowMessageBoxButton button, ShowMessageBoxIcon icon)
		{
			MessageBoxResult result = MessageBox.Show(message, title, ConvertShowMessageBoxButton(button), ConvertShowMessageBoxIcon(icon));
			return ConvertShowMessageBoxResult(result);
		}

		private static MessageBoxButton ConvertShowMessageBoxButton(ShowMessageBoxButton button)
		{
			switch (button)
			{
				case ShowMessageBoxButton.Ok:
					return MessageBoxButton.OK;

				case ShowMessageBoxButton.OkCancel:
					return MessageBoxButton.OKCancel;

				case ShowMessageBoxButton.YesNoCancel:
					return MessageBoxButton.YesNoCancel;

				case ShowMessageBoxButton.YesNo:
					return MessageBoxButton.YesNo;

				default:
					throw new NotSupportedException($"The MessageBox button type {button} is not supported");
			}
		}

		private static MessageBoxImage ConvertShowMessageBoxIcon(ShowMessageBoxIcon icon)
		{
			switch (icon)
			{
				case ShowMessageBoxIcon.NoImage:
					return MessageBoxImage.None;

				case ShowMessageBoxIcon.Information:
					return MessageBoxImage.Information;

				case ShowMessageBoxIcon.Warning:
					return MessageBoxImage.Warning;

				case ShowMessageBoxIcon.Error:
					return MessageBoxImage.Error;

				case ShowMessageBoxIcon.Question:
					return MessageBoxImage.Question;

				case ShowMessageBoxIcon.Exclamation:
					return MessageBoxImage.Exclamation;

				default:
					throw new NotSupportedException($"The MessageBox icon type {icon} is not supported");
			}
		}

		private static ShowMessageBoxResult ConvertShowMessageBoxResult(MessageBoxResult result)
		{
			switch (result)
			{
				case MessageBoxResult.None:
					return ShowMessageBoxResult.NoResult;

				case MessageBoxResult.OK:
					return ShowMessageBoxResult.Ok;

				case MessageBoxResult.Cancel:
					return ShowMessageBoxResult.Cancel;

				case MessageBoxResult.Yes:
					return ShowMessageBoxResult.Yes;

				case MessageBoxResult.No:
					return ShowMessageBoxResult.No;

				default:
					throw new NotSupportedException($"The MessageBox result type {result} is not supported");
			}
		}
	}
}
