using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CF.Library.Patterns
{
	/// <summary>
	/// Cache class that supports asynchronous value factory
	/// </summary>
	public class CacheAsync<TKey, TValue>
	{
		private readonly Dictionary<TKey, TValue> cache;

		/// <summary>
		/// Constructor
		/// </summary>
		public CacheAsync()
		{
			cache = new Dictionary<TKey, TValue>();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CacheAsync(IEqualityComparer<TKey> comparer)
		{
			cache = new Dictionary<TKey, TValue>(comparer);
		}

		/// <summary>
		/// Gets value from the cache or loads it asynchronously with provided value factory
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need possibility to have asynchronous value factory")]
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

		private bool TryGetValue(TKey key, out TValue value)
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
		private void SetValue(TKey key, TValue value)
		{
			lock (cache)
			{
				cache[key] = value;
			}
		}
	}
}
