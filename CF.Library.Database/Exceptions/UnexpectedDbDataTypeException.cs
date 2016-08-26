using System;
using System.Runtime.Serialization;

namespace CF.Library.Database.Exceptions
{
	/// <summary>
	/// The exception that is thrown when type of fetched data from DB is unexpected
	/// </summary>
	[Serializable]
	public class UnexpectedDbDataTypeException : BasicDbException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedDbDataTypeException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedDbDataTypeException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedDbDataTypeException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected UnexpectedDbDataTypeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
