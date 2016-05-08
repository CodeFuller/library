using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Patterns
{
	public class CacheAsync<TKey, TValue>
	{
		private readonly Dictionary<TKey, TValue> cache;

		public CacheAsync()
		{
			cache = new Dictionary<TKey, TValue>();
		}

		public CacheAsync(IEqualityComparer<TKey> comparer)
		{
			cache = new Dictionary<TKey, TValue>(comparer);
		}

		public async Task<TValue> GetValue(TKey key, Func<Task<TValue>> valueFactory)
		{
			//	Searching in the cache
			TValue value;
			if (TryGetValue(key, out value))
			{
				return value;
			}

			value = await valueFactory();

			//	Storing value in the cache
			SetValue(key, value);

			return value;
		}

		bool TryGetValue(TKey key, out TValue value)
		{
			lock (cache)
			{
				return cache.TryGetValue(key, out value);
			}
		}

		/// <summary>
		/// Puts data in the cache
		/// </summary>
		/// <remarks>
		/// It's possible that this method is called when this data already exist in the cache
		/// In this case it should override existing value
		/// </remarks>
		void SetValue(TKey key, TValue value)
		{
			lock (cache)
			{
				cache[key] = value;
			}
		}
	}
}
