using System;
using System.Windows;
using CodeFuller.Library.Wpf.Interfaces;

namespace CodeFuller.Library.Wpf.Internal
{
	internal class WpfWindowService : IWindowService
	{
		public ShowMessageBoxResult ShowMessageBox(string message)
		{
			var result = MessageBox.Show(message);
			return ConvertShowMessageBoxResult(result);
		}

		public ShowMessageBoxResult ShowMessageBox(string message, string title, ShowMessageBoxButton button, ShowMessageBoxIcon icon)
		{
			var result = MessageBox.Show(message, title, ConvertShowMessageBoxButton(button), ConvertShowMessageBoxIcon(icon));
			return ConvertShowMessageBoxResult(result);
		}

		private static MessageBoxButton ConvertShowMessageBoxButton(ShowMessageBoxButton button)
		{
			return button switch
			{
				ShowMessageBoxButton.Ok => MessageBoxButton.OK,
				ShowMessageBoxButton.OkCancel => MessageBoxButton.OKCancel,
				ShowMessageBoxButton.YesNoCancel => MessageBoxButton.YesNoCancel,
				ShowMessageBoxButton.YesNo => MessageBoxButton.YesNo,
				_ => throw new NotSupportedException($"The MessageBox button type {button} is not supported")
			};
		}

		private static MessageBoxImage ConvertShowMessageBoxIcon(ShowMessageBoxIcon icon)
		{
			return icon switch
			{
				ShowMessageBoxIcon.NoImage => MessageBoxImage.None,
				ShowMessageBoxIcon.Information => MessageBoxImage.Information,
				ShowMessageBoxIcon.Warning => MessageBoxImage.Warning,
				ShowMessageBoxIcon.Error => MessageBoxImage.Error,
				ShowMessageBoxIcon.Question => MessageBoxImage.Question,
				ShowMessageBoxIcon.Exclamation => MessageBoxImage.Exclamation,
				_ => throw new NotSupportedException($"The MessageBox icon type {icon} is not supported")
			};
		}

		private static ShowMessageBoxResult ConvertShowMessageBoxResult(MessageBoxResult result)
		{
			return result switch
			{
				MessageBoxResult.None => ShowMessageBoxResult.NoResult,
				MessageBoxResult.OK => ShowMessageBoxResult.Ok,
				MessageBoxResult.Cancel => ShowMessageBoxResult.Cancel,
				MessageBoxResult.Yes => ShowMessageBoxResult.Yes,
				MessageBoxResult.No => ShowMessageBoxResult.No,
				_ => throw new NotSupportedException($"The MessageBox result type {result} is not supported")
			};
		}
	}
}
