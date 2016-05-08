using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	public class BasicException : Exception
	{
		public BasicException()
		{
		}

		public BasicException(string message, params object[] args)
			: base(String.Format(message, args))
		{
		}
	}
}
