using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CF.Core.Exceptions;

namespace CF.Core.Exceptions.Database
{
	/// <summary>
	/// The exception that is thrown when value of fetched data from DB is unexpected
	/// </summary>
	[Serializable]
	public class UnexpectedDbDataException : BasicDbException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedDbDataException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedDbDataException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedDbDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected UnexpectedDbDataException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
