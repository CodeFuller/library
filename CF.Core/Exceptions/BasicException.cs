using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	/// <summary>
	/// Base class for all exceptions in CF.Library
	/// </summary>
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
		public BasicException(string message, params object[] args)
			: base(String.Format(message, args))
		{
		}
	}
}
