using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
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
