using System.Configuration;

namespace CF.Testing
{
	/// <summary>
	/// Container for configuration helper methods.
	/// </summary>
	public static class ConfigurationHelpers
	{
		/// <summary>
		/// Adds or updates specified configuration section.
		/// </summary>
		public static void SetConfigurationSection(string sectionName, ConfigurationSection section)
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.Sections.Remove(sectionName);
			config.Sections.Add(sectionName, section);
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection(sectionName);
		}

		/// <summary>
		/// Removes specified configuration section.
		/// </summary>
		public static void RemoveConfigurationSection(string sectionName)
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.Sections.Remove(sectionName);
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection(sectionName);
		}
	}
}
