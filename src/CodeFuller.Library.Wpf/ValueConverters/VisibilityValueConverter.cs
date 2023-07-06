using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CodeFuller.Library.Wpf.ValueConverters
{
	/// <summary>
	/// Value converter for converting boolean value to <see cref="Visibility"/>.
	/// </summary>
	public abstract class VisibilityValueConverter : IValueConverter
	{
		/// <summary>
		/// Gets the value of <see cref="Visibility"/> which should be returned for disabled control.
		/// </summary>
		protected abstract Visibility DisabledControlVisibility { get; }

		/// <summary>
		/// Gets or sets a value indicating whether the visibility is inverted, i.e. control is visible if converted property is set to false.
		/// </summary>
		public bool InvertedVisibility { get; set; }

		/// <summary>
		/// Converts boolean value to <see cref="Visibility"/>.
		/// </summary>
		/// <param name="value">Boolean value indicating whether the control should be enabled or disabled.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// The value of <see cref="Visibility"/> corresponding to input boolean value.
		/// If true, then <see cref="Visibility.Visible" /> is returned.
		/// Otherwise, the value of <see cref="DisabledControlVisibility"/> is returned.
		/// </returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is not bool isVisibleValue || targetType != typeof(Visibility))
			{
				return DependencyProperty.UnsetValue;
			}

			return isVisibleValue ^ InvertedVisibility ? Visibility.Visible : DisabledControlVisibility;
		}

		/// <summary>
		/// This method is not implemented for the converter. The value <see cref="DependencyProperty.UnsetValue"/> is always returned.
		/// </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>The value <see cref="DependencyProperty.UnsetValue"/> is always returned.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
