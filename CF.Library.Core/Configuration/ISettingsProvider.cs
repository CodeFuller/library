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
	}
}
