namespace CF.Library.Core.Configuration
{
	/// <summary>
	/// Interface for application settings provider.
	/// </summary>
	public interface ISettingsProvider
	{
		/// <summary>
		/// Returns required setting value.
		/// Throws RequiredSettingIsMissingException if setting is missing.
		/// </summary>
		T GetRequiredValue<T>(string key);

		/// <summary>
		/// Returns optional value or default(T) if setting is missing.
		/// </summary>
		T GetOptionalValue<T>(string key) where T : class;

		/// <summary>
		/// Returns required setting value specific for current user.
		/// Throws RequiredSettingIsMissingException if setting is missing.
		/// </summary>
		T GetPrivateRequiredValue<T>(string key);

		/// <summary>
		/// Returns optional value specific for current user or default(T) if setting is missing.
		/// </summary>
		T GetPrivateOptionalValue<T>(string key) where T : class;
	}
}
