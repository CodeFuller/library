using System;
using System.Collections.Generic;

namespace CF.Library.Core.Extensions
{
	/// <summary>
	/// Holder for collection extension methods
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Returns value for specific key in the dictionary. Default value is added for the key if required.
		/// </summary>
		public static TValue ProvideValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TValue : new()
		{
			if (dict == null)
			{
				throw new ArgumentNullException(nameof(dict));
			}

			if (!dict.TryGetValue(key, out var value))
			{
				value = new TValue();
				dict.Add(key, value);
			}
			return value;
		}
	}
}
