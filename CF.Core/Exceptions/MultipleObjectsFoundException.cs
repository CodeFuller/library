using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	public class MultipleObjectsFoundException : BasicException
	{
		public MultipleObjectsFoundException()
		{
		}

		public MultipleObjectsFoundException(string message, params object[] args)
			: base(String.Format(message, args))
		{
		}
	}
}
