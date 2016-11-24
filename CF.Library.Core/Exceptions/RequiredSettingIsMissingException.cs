using System;
using System.Runtime.Serialization;

namespace CF.Library.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when required application settings is missing.
	/// </summary>
	[Serializable]
	public class RequiredSettingIsMissingException : InvalidConfigurationException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public RequiredSettingIsMissingException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public RequiredSettingIsMissingException(string keyName)
			: base(FormattableString.Invariant($"Required setting '{keyName}' is missing"))
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public RequiredSettingIsMissingException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected RequiredSettingIsMissingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
