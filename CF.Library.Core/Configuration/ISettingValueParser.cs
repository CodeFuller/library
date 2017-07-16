namespace CF.Library.Core.Configuration
{
	/// <summary>
	/// Interface for parsing application setting values.
	/// </summary>
	public interface ISettingValueParser
	{
		/// <summary>
		/// Parses string setting value to strongly typed value.
		/// </summary>
		T Parse<T>(string value);
	}
}
