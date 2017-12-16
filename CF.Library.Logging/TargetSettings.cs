using System;
using System.Collections.Generic;
using System.Globalization;

namespace CF.Library.Logging
{
	public class TargetSettings : Dictionary<string, string>
	{
		public T Get<T>(string key)
		{
			if (!TryGetValue(key, out var stringValue))
			{
				throw new InvalidOperationException($"Required target setting '{key}' is not set");
			}

			return ConvertValue<T>(stringValue);
		}

		public T GetOptional<T>(string key)
		{
			if (!TryGetValue(key, out var stringValue))
			{
				return default(T);
			}

			return ConvertValue<T>(stringValue);
		}

		private static T ConvertValue<T>(string value)
		{
			Type t = typeof(T);

			if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				t = Nullable.GetUnderlyingType(t);
			}

			return (T)Convert.ChangeType(value, t, CultureInfo.InvariantCulture);
		}
	}
}
