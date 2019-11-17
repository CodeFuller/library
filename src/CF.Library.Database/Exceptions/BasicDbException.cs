using System;
using System.Runtime.Serialization;
using CF.Library.Core.Exceptions;

namespace CF.Library.Database.Exceptions
{
	/// <summary>
	/// Basic class for Database exceptions
	/// </summary>
	[Serializable]
	public class BasicDbException : BasicException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public BasicDbException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public BasicDbException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public BasicDbException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected BasicDbException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
