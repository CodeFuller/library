using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CF.Core.Exceptions;
using CF.Core.Exceptions.Database;

namespace CF.Core.Exceptions.Database
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
