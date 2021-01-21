using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CodeFuller.Library.Wpf.Extensions
{
	/// <summary>
	/// Holder for DependencyObject extension methods.
	/// </summary>
	public static class DependencyObjectExtensions
	{
		/// <summary>
		/// Checks whether any of child controls have validation errors.
		/// </summary>
		/// <remarks>
		/// https://stackoverflow.com/a/4650392/5740031
		/// </remarks>
		public static bool IsValid(this DependencyObject obj)
		{
			return !Validation.GetHasError(obj) &&
			LogicalTreeHelper.GetChildren(obj)
			.OfType<DependencyObject>()
			.All(IsValid);
		}
	}
}
