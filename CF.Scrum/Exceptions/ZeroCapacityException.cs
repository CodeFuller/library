using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Scrum.Exceptions
{
	public class ZeroCapacityException : ScrumException
	{
		public ZeroCapacityException()
		{
		}

		public ZeroCapacityException(string message, params object[] args)
			: base(String.Format(message, args))
		{
		}
	}
}
