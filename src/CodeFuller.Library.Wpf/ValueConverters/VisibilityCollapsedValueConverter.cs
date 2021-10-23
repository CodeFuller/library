using System.Windows;

namespace CodeFuller.Library.Wpf.ValueConverters
{
	/// <summary>
	/// Value converter for converting boolean value to <see cref="Visibility"/>. False input value is converted to <see cref="Visibility.Collapsed"/>.
	/// </summary>
	public class VisibilityCollapsedValueConverter : VisibilityValueConverter
	{
		/// <summary>
		/// Gets the value of <see cref="Visibility"/> which should be returned for disabled control. For current class it's always <see cref="Visibility.Collapsed"/>.
		/// </summary>
		protected override Visibility DisabledControlVisibility => Visibility.Collapsed;
	}
}
