using System;
using System.Runtime.Serialization;

namespace CF.Library.Database.Exceptions
{
	/// <summary>
	/// The exception that is thrown when SELECT returned extra data that was not expected
	/// </summary>
	[Serializable]
	public class ExtraDbDataException : BasicDbException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExtraDbDataException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ExtraDbDataException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ExtraDbDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected ExtraDbDataException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
