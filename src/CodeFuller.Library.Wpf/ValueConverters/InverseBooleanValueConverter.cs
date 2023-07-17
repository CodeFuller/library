using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CodeFuller.Library.Wpf.ValueConverters
{
    /// <summary>
    /// Value converter for converting boolean value to inverted value.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanValueConverter : IValueConverter
    {
        /// <summary>
        /// Converts boolean value to inverted value.
        /// </summary>
        /// <param name="value">Boolean value for inversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// true, if value argument is false.
        /// false, if value argument is true.
        /// <see cref="DependencyProperty.UnsetValue"/> if value is not boolean.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool booleanValue || targetType != typeof(bool))
            {
                return DependencyProperty.UnsetValue;
            }

            return !booleanValue;
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
