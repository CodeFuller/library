using System;
using System.Globalization;

namespace CF.Library.Core.Configuration
{
	/// <summary>
	/// Implementation for ISettingValueParser.
	/// </summary>
	public class SettingValueParser : ISettingValueParser
	{
		/// <summary>
		/// Implementation for ISettingValueParser.Parse().
		/// </summary>
		public T Parse<T>(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			if (typeof(T) == typeof(DataSize))
			{
				return (T)(object)new DataSize(value);
			}

			//	Currently such implementation is sufficient.
			//	It could be extended for some specific types if required.
			return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
		}
	}
}
