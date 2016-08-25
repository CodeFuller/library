﻿using System;
using System.Collections.Generic;

namespace CF.Core
{
	/// <summary>
	/// Holder for collection extension methods
	/// </summary>
	public static class CollectionExtensions
	{
		/// <summary>
		/// Adds default value for specific key to the dictionary if it doesn't exist
		/// </summary>
		public static void FillDefaultValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TValue : new()
		{
			if (dict == null)
			{
				throw new ArgumentNullException(nameof(dict));
			}

			if (!dict.ContainsKey(key))
			{
				dict.Add(key, new TValue());
			}
		}

		/// <summary>
		/// Returns value for specific key in the dictionary if it exists
		/// Otherwise returns default value of the type specified
		/// </summary>
		public static TValue SafeGet<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
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
		/// Returns value for specific key in the dictionary if it exists
		/// Otherwise adds and returns default value of the type specified
		/// </summary>
		public static TValue ProvideValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TValue : new()
		{
			if (dict == null)
			{
				throw new ArgumentNullException(nameof(dict));
			}

			lock (dict)
			{
				TValue value;
				if (!dict.TryGetValue(key, out value))
				{
					value = new TValue();
					dict.Add(key, value);
				}
				return value;
			}
		}

		/// <summary>
		/// Adds given value to the value stored at specified key in the dictionary
		/// If value doesn't yet exist - just saves given value in the dictionary
		/// </summary>
		public static void SafeAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
		{
			if (dict == null)
			{
				throw new ArgumentNullException(nameof(dict));
			}

			if (dict.ContainsKey(key))
			{
				dynamic v1 = dict[key];
				dynamic v2 = value;
				dict[key] = v1 + v2;
			}
			else
			{
				dict.Add(key, value);
			}
		}
	}
}
