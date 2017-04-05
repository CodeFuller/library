using System;
using System.Runtime.Serialization;

namespace CF.Library.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when several objects were found where only one expected.
	/// </summary>
	[Serializable]
	public class MultipleObjectsFoundException : BasicException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public MultipleObjectsFoundException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public MultipleObjectsFoundException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public MultipleObjectsFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected MultipleObjectsFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
