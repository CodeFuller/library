using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	public class InternalErrorException : BasicException
	{
		public InternalErrorException()
		{
		}

		public InternalErrorException(string message, params object[] args)
			: base(String.Format(message, args))
		{
		}
	}
}
