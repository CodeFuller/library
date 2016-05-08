using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	public class ObjectNotFoundException : BasicException
	{
		public ObjectNotFoundException()
		{
		}

		public ObjectNotFoundException(string objectName)
			: this(String.Format("Object '{0}' not found"), objectName)
		{
		}

		public ObjectNotFoundException(string message, params object[] args)
			: base(String.Format(message, args))
		{
		}
	}
}
