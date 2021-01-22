using System;
using System.Collections.Generic;
using System.Globalization;

namespace CodeFuller.Library.Logging.Internal
{
	internal static class DictionaryExtensions
	{
		public static T GetOptionalSetting<T>(this IDictionary<string, string> targetSettings, string key)
		{
			return !targetSettings.TryGetValue(key, out var stringValue) ? default : ConvertValue<T>(stringValue);
		}

		private static T ConvertValue<T>(string value)
		{
			var type = typeof(T);

			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = Nullable.GetUnderlyingType(type);
			}

			if (type == typeof(TimeSpan))
			{
				return (T)(object)TimeSpan.Parse(value, CultureInfo.InvariantCulture);
			}

			if (type?.BaseType == typeof(Enum))
			{
				return (T)Enum.Parse(type, value, true);
			}

			return (T)Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
		}
	}
}
