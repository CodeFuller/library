using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	public class UnexpectedEnumValueException : BasicException
	{
		public UnexpectedEnumValueException()
		{
		}

		public UnexpectedEnumValueException(Enum e)
			: base(String.Format("Unexpected enum value: {0}", e))
		{
		}
	}
}
