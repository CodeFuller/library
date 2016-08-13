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
	/// The exception that is thrown when SELECT returned no data but some data was expected
	/// </summary>
	[Serializable]
	public class NoFetchedDbDataException : BasicDbException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public NoFetchedDbDataException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public NoFetchedDbDataException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public NoFetchedDbDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected NoFetchedDbDataException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
