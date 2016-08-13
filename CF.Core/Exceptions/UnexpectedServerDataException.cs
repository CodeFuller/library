using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when server response contains unexpected data
	/// </summary>
	[Serializable]
	public class UnexpectedServerDataException : BasicException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedServerDataException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedServerDataException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedServerDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected UnexpectedServerDataException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
