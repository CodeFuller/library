using System.Collections.Generic;

namespace CodeFuller.Library.Core.Extensions
{
	public static class CollectionExtensions
	{
		/// <summary>
		/// Adds the elements to the end of the collection.
		/// </summary>
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
		{
			foreach (var item in newItems)
			{
				collection.Add(item);
			}
		}
	}
}
