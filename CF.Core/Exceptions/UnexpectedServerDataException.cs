using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when server response contains unexpected data
	/// </summary>
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
		public UnexpectedServerDataException(string message, params object[] args)
			: base(String.Format(message, args))
		{
		}
	}
}
