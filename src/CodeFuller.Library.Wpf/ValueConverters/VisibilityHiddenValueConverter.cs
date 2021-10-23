using System.Windows;

namespace CodeFuller.Library.Wpf.ValueConverters
{
	/// <summary>
	/// Value converter for converting boolean value to <see cref="Visibility"/>. False input value is converted to <see cref="Visibility.Hidden"/>.
	/// </summary>
	public class VisibilityHiddenValueConverter : VisibilityValueConverter
	{
		/// <summary>
		/// Gets the value of <see cref="Visibility"/> which should be returned for disabled control. For current class it's always <see cref="Visibility.Hidden"/>.
		/// </summary>
		protected override Visibility DisabledControlVisibility => Visibility.Hidden;
	}
}
