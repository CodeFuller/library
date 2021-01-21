using System;
using System.Runtime.Serialization;

namespace CodeFuller.Library.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when some internal error occurs in the program.
	/// </summary>
	[Serializable]
	public class InternalErrorException : BasicException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InternalErrorException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public InternalErrorException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public InternalErrorException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected InternalErrorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
