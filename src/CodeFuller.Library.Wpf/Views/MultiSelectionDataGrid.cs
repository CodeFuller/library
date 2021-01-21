using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CodeFuller.Library.Wpf.Views
{
	// https://stackoverflow.com/a/22908694/5740031
	public class MultiSelectionDataGrid : DataGrid
	{
		public static readonly DependencyProperty SelectedItemsListProperty =
			DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(MultiSelectionDataGrid), new PropertyMetadata(null));

		public IList SelectedItemsList
		{
			get => (IList)GetValue(SelectedItemsListProperty);
			set => SetValue(SelectedItemsListProperty, value);
		}

		public MultiSelectionDataGrid()
		{
			SelectionChanged += CustomDataGrid_SelectionChanged;
			KeyUp += DataGrid_KeyUp;
		}

		private void CustomDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedItemsList = SelectedItems;
		}

		private void DataGrid_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				(sender as DataGrid)?.UnselectAll();
			}
		}
	}
}
