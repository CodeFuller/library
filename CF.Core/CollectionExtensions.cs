using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core
{
	public static class CollectionExtensions
	{
		public static void FillDefaultValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TValue : new()
		{
			if (!dict.ContainsKey(key))
			{
				dict.Add(key, new TValue());
			}
		}

		public static void SafeAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
		{
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
