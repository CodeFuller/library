using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when some internal error occurs in the program
	/// </summary>
	public class InternalErrorException : BasicException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InternalErrorException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public InternalErrorException(string message, params object[] args)
			: base(String.Format(message, args))
		{
		}
	}
}
