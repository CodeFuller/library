using System;
using System.Configuration;
using System.IO;
using CF.Library.Core.Exceptions;
using static CF.Library.Core.Extensions.FormattableStringExtensions;

namespace CF.Library.Core.Configuration
{
	/// <summary>
	/// Settings provider that reads settings from config file.
	/// </summary>
	public class FileSettingsProvider : BasicSettingsProvider
	{
		private const string AppDataPathSettingKey = "AppDataPath";

		/// <summary>
		/// Returns setting value from config file.
		/// </summary>
		protected override string GetSettingValue(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}

		/// <summary>
		/// Returns setting value from user private config file.
		/// </summary>
		protected override string GetPrivateSettingValue(string key)
		{
			var userConfigFileName = GetUserConfigFileName();
			if (!File.Exists(userConfigFileName))
			{
				throw new InvalidOperationException(Current($"User config file is missing: '{userConfigFileName}'"));
			}

			ConfigurationFileMap fileMap = new ConfigurationFileMap(GetUserConfigFileName());
			System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedMachineConfiguration(fileMap);

			return configuration.AppSettings.Settings[key].Value;
		}

		private static string GetUserConfigFileName()
		{
			var appDataPath = ConfigurationManager.AppSettings[AppDataPathSettingKey];
			if (String.IsNullOrEmpty(appDataPath))
			{
				throw new RequiredSettingIsMissingException(AppDataPathSettingKey);
			}

			return Path.Combine(appDataPath, "AppUser.config");
		}
	}
}
