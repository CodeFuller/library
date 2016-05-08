using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	public class SystemCallFailedException : BasicException
	{
		public SystemCallFailedException(int iSystemError)
		{

		}
	}
}
