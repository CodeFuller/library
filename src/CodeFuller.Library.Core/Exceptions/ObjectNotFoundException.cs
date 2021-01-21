using System;
using System.Runtime.Serialization;

namespace CodeFuller.Library.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when requested object is not found.
	/// </summary>
	[Serializable]
	public class ObjectNotFoundException : BasicException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectNotFoundException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectNotFoundException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected ObjectNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
