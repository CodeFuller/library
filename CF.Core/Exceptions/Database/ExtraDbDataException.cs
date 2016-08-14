﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CF.Core.Exceptions;

namespace CF.Core.Exceptions.Database
{
	/// <summary>
	/// The exception that is thrown when SELECT returned extra data that was not expected
	/// </summary>
	[Serializable]
	public class ExtraDbDataException : BasicDbException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ExtraDbDataException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ExtraDbDataException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ExtraDbDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected ExtraDbDataException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}