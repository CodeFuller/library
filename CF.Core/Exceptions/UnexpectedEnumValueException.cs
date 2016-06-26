using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when unexpected enum value is encountered
	/// </summary>
	public class UnexpectedEnumValueException : BasicException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedEnumValueException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedEnumValueException(Enum e)
			: base(String.Format("Unexpected enum value: {0}", e))
		{
		}
	}
}
