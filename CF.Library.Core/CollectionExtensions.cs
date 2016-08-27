using System;
using System.Collections.Generic;

namespace CF.Library.Core
{
	/// <summary>
	/// Holder for collection extension methods
	/// </summary>
	public static class CollectionExtensions
	{
		/// <summary>
		/// Returns value for specific key in the dictionary if it exists.
		/// Otherwise returns default value of the type specified.
		/// </summary>
		public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
		{
			if (dict == null)
			{
				throw new ArgumentNullException(nameof(dict));
			}

			TValue value;
			dict.TryGetValue(key, out value);
			return value;
		}

		/// <summary>
		/// Returns value for specific key in the dictionary. Default value is added for the key if required.
		/// </summary>
		public static TValue ProvideValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TValue : new()
		{
			if (dict == null)
			{
				throw new ArgumentNullException(nameof(dict));
			}

			TValue value;
			if (!dict.TryGetValue(key, out value))
			{
				value = new TValue();
				dict.Add(key, value);
			}
			return value;
		}
	}
}
