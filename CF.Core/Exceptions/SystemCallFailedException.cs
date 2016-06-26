using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when some system call (e.g. Windows API) fails
	/// </summary>
	public class SystemCallFailedException : BasicException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SystemCallFailedException(int iSystemError)
		{

		}
	}
}
