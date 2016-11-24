using System;
using System.Runtime.Serialization;

namespace CF.Library.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when application configuration is invalid.
	/// </summary>
	[Serializable]
	public class InvalidConfigurationException : BasicException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InvalidConfigurationException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public InvalidConfigurationException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public InvalidConfigurationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected InvalidConfigurationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
