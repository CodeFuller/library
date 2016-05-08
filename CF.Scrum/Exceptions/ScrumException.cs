using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Core.Exceptions;

namespace CF.Scrum.Exceptions
{
	public class ScrumException : BasicException
	{
		public ScrumException()
		{
		}

		public ScrumException(string message, params object[] args)
			: base(String.Format(message, args))
		{
		}
	}
}
