using System;
using System.Globalization;
using System.Windows.Data;

namespace CodeFuller.Library.Wpf.ValueConverters
{
	/// <remarks>
	/// Copy/paste from http://stackoverflow.com/a/1039681/5740031
	/// </remarks>>
	[ValueConversion(typeof(bool), typeof(bool))]
	public class InverseBooleanConverter : IValueConverter
	{
		/// <summary>
		/// Converts value.
		/// </summary>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(bool))
			{
				throw new InvalidOperationException("The target must be a boolean");
			}

			return !(bool)value;
		}

		/// <summary>
		/// ConvertBack() method is not implemented for this converter.
		/// </summary>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
