using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CodeFuller.Library.Wpf.Controls
{
	/// <summary>
	/// Custom <see cref="ListBox"/> with property for multiple selected items.
	/// </summary>
	public class MultiSelectionListBox : ListBox
	{
		/// <summary>
		/// Dependency property for <see cref="SelectedItemsList"/>.
		/// </summary>
		public static readonly DependencyProperty SelectedItemsListProperty =
			DependencyProperty.Register(nameof(SelectedItemsList), typeof(IList), typeof(MultiSelectionListBox), new PropertyMetadata(null));

		/// <summary>
		/// Gets or sets the list of selected items.
		/// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
		public IList SelectedItemsList
#pragma warning restore CA2227 // Collection properties should be read only
		{
			get => (IList)GetValue(SelectedItemsListProperty);
			set => SetValue(SelectedItemsListProperty, value);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiSelectionListBox"/> class.
		/// </summary>
		public MultiSelectionListBox()
		{
			SelectionChanged += ListBox_SelectionChanged;
			KeyUp += ListBox_KeyUp;
		}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedItemsList = SelectedItems;
		}

		private static void ListBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				(sender as ListBox)?.UnselectAll();
			}
		}
	}
}
