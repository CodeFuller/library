using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeFuller.Library.Logging
{
	// .NET Core configuration binder does not support binding of Dictionary with case-insensitive keys.
	// If you use usual Dictionary, you'll get separate values for the keys, which differ only by the case (e.g. "logPath" and "LogPath").
	// Some of these values will be bind to the settings object, but configuration overwrite rules (the last configuration source wins) will not work.
	// If you use case-insensitive dictionary (constructed with StringComparer.OrdinalIgnoreCase),
	// you'll catch ArgumentException: An item with the same key has already been added.
	//
	// This implementation of dictionary replicates usual dictionary (case insensitive),
	// except for Add() method which overwrites existing value with new one.
	public class ConfigurationSafeDictionary<TValue> : IDictionary<string, TValue>
	{
		private readonly Dictionary<string, TValue> _data = new Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase);

		private ICollection<KeyValuePair<string, TValue>> Collection => _data;

		public int Count => _data.Count;

		public bool IsReadOnly => Collection.IsReadOnly;

		public TValue this[string key]
		{
			get => _data[key];
			set => _data[key] = value;
		}

		public ICollection<string> Keys => _data.Keys;

		public ICollection<TValue> Values => _data.Values;

		public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
		{
			return _data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(KeyValuePair<string, TValue> item)
		{
			Collection.Add(item);
		}

		public void Clear()
		{
			_data.Clear();
		}

		public bool Contains(KeyValuePair<string, TValue> item)
		{
			return Collection.Contains(item);
		}

		public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
		{
			Collection.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, TValue> item)
		{
			return Collection.Remove(item);
		}

		public void Add(string key, TValue value)
		{
			_data[key] = value;
		}

		public bool ContainsKey(string key)
		{
			return _data.ContainsKey(key);
		}

		public bool Remove(string key)
		{
			return _data.Remove(key);
		}

		public bool TryGetValue(string key, out TValue value)
		{
			return _data.TryGetValue(key, out value);
		}
	}
}
