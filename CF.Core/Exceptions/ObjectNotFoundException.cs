using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when requested object is not found
	/// </summary>
	public class ObjectNotFoundException : BasicException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectNotFoundException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectNotFoundException(string objectName)
			: this(String.Format("Object '{0}' not found"), objectName)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ObjectNotFoundException(string message, params object[] args)
			: base(String.Format(message, args))
		{
		}
	}
}
