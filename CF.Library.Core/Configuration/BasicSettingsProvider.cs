using CF.Library.Core.Exceptions;

namespace CF.Library.Core.Configuration
{
	/// <summary>
	/// Basic class for setting providers.
	/// </summary>
	public abstract class BasicSettingsProvider : ISettingsProvider
	{
		/// <summary>
		/// Property Injection for ISettingValueParser.
		/// </summary>
		internal ISettingValueParser SettingValueParser { get; set; } = new SettingValueParser();

		/// <summary>
		/// Implementation for ISettingsProvider.GetRequiredValue()
		/// </summary>
		public T GetRequiredValue<T>(string key)
		{
			string value = GetSettingValue(key);
			if (value == null)
			{
				throw new RequiredSettingIsMissingException(key);
			}

			return SettingValueParser.Parse<T>(value);
		}

		/// <summary>
		/// Implementation for ISettingsProvider.GetOptionalValue().
		/// </summary>
		public T GetOptionalValue<T>(string key) where T : class
		{
			string value = GetSettingValue(key);
			return value != null ? SettingValueParser.Parse<T>(value) : null;
		}

		/// <summary>
		/// Returns string setting value stored in provider or null if setting is missing.
		/// </summary>
		protected abstract string GetSettingValue(string key);
	}
}
