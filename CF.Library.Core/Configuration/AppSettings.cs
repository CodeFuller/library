using System;
using System.Collections.Generic;

namespace CF.Library.Core.Configuration
{
	/// <summary>
	/// Cross-cutting holder for application settings.
	/// </summary>
	public static class AppSettings
	{
		private static ISettingsProvider settingsProvider;

		/// <summary>
		/// Property Injection for ISettingsProvider.
		/// </summary>
		public static ISettingsProvider SettingsProvider
		{
			get
			{
				if (settingsProvider == null)
				{
					throw new InvalidOperationException("Settings Provider is not set");
				}

				return settingsProvider;
			}

			set
			{
				if (settingsProvider != null)
				{
					throw new InvalidOperationException("Settings Provider is already set");
				}

				settingsProvider = value;
			}
		}

		/// <summary>
		/// This method is required for tests only to have possibility setting SettingsProvider multiple times within one process.
		/// </summary>
		public static void ResetSettingsProvider()
		{
			settingsProvider = null;
		}

		/// <summary>
		/// Returns required setting value.
		/// Throws RequiredSettingIsMissingException if setting is missing.
		/// </summary>
		public static T GetRequiredValue<T>(string key)
		{
			return SettingsProvider.GetRequiredValue<T>(key);
		}

		/// <summary>
		/// Returns optional value or default(T) if setting is missing.
		/// </summary>
		public static T GetOptionalValue<T>(string key) where T : class
		{
			return SettingsProvider.GetOptionalValue<T>(key);
		}

		/// <summary>
		/// Returns list of optional values specified as key1, key2, ..., keyn.
		/// </summary>
		public static IEnumerable<T> GetOptionalValues<T>(string key) where T : class
		{
			for (var i = 1; ; ++i)
			{
				T currValue = GetOptionalValue<T>($"{key}{i}");
				if (currValue == default(T))
				{
					yield break;
				}

				yield return currValue;
			}
		}

		/// <summary>
		/// Returns required setting value specific for current user.
		/// Throws RequiredSettingIsMissingException if setting is missing.
		/// </summary>
		public static T GetPrivateRequiredValue<T>(string key)
		{
			return SettingsProvider.GetPrivateRequiredValue<T>(key);
		}

		/// <summary>
		/// Returns optional value specific for current user or default(T) if setting is missing.
		/// </summary>
		public static T GetPrivateOptionalValue<T>(string key) where T : class
		{
			return SettingsProvider.GetPrivateOptionalValue<T>(key);
		}
	}
}
