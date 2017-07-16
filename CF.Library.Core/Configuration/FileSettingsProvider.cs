using System.Configuration;

namespace CF.Library.Core.Configuration
{
	/// <summary>
	/// Settings provider that reads settings from config file.
	/// </summary>
	public class FileSettingsProvider : BasicSettingsProvider
	{
		/// <summary>
		/// Returns setting value from config file.
		/// </summary>
		protected override string GetSettingValue(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
	}
}
