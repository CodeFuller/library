using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CodeFuller.Library.Wpf.Controls
{
	/// <summary>
	/// Custom <see cref="DataGrid"/> with property for multiple selected items.
	/// </summary>
	// https://stackoverflow.com/a/22908694/5740031
	public class MultiSelectionDataGrid : DataGrid
	{
		/// <summary>
		/// Dependency property for <see cref="SelectedItemsList"/>.
		/// </summary>
		public static readonly DependencyProperty SelectedItemsListProperty =
			DependencyProperty.Register(nameof(SelectedItemsList), typeof(IList), typeof(MultiSelectionDataGrid), new PropertyMetadata(null));

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
		/// Initializes a new instance of the <see cref="MultiSelectionDataGrid"/> class.
		/// </summary>
		public MultiSelectionDataGrid()
		{
			SelectionChanged += CustomDataGrid_SelectionChanged;
			KeyUp += DataGrid_KeyUp;
		}

		private void CustomDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedItemsList = SelectedItems;
		}

		private static void DataGrid_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				(sender as DataGrid)?.UnselectAll();
			}
		}
	}
}
