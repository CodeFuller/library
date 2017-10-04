using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CF.Library.Core.Extensions
{
	/// <summary>
	/// Holder for ObservableCollection extension methods.
	/// </summary>
	public static class ObservableCollectionExtensions
	{
		/// <summary>
		/// Adds new items to the end of ObservableCollection.
		/// </summary>
		public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> newItems)
		{
			foreach (var item in newItems)
			{
				collection.Add(item);
			}
		}
	}
}
