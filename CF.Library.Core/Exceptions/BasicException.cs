using System;
using System.Runtime.Serialization;

namespace CF.Library.Core.Exceptions
{
	/// <summary>
	/// Base class for all exceptions in CF.Library
	/// </summary>
	[Serializable]
	public class BasicException : Exception
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public BasicException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public BasicException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public BasicException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected BasicException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
