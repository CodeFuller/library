using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when several objects were found where only one expected
	/// </summary>
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
		public MultipleObjectsFoundException(string message, params object[] args)
			: base(String.Format(message, args))
		{
		}
	}
}
