using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeFuller.Library.Core.Extensions
{
	/// <summary>
	/// Holder for IEnumerable extension methods
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Creates collection from IEnumerable.
		/// </summary>
		public static Collection<T> ToCollection<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}
			return new Collection<T>(source.ToList());
		}

		/// <summary>
		/// Creates collection from IEnumerable.
		/// </summary>
		public static T UniqueOrDefault<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			var distinctValues = source.Distinct().ToList();
			return distinctValues.Count == 1 ? distinctValues[0] : default(T);
		}
	}
}
