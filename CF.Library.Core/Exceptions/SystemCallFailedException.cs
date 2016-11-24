using System;
using System.Runtime.Serialization;

namespace CF.Library.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when some system call (e.g. Windows API) fails.
	/// </summary>
	[Serializable]
	public class SystemCallFailedException : BasicException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SystemCallFailedException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public SystemCallFailedException(int systemErrorCode)
			: base(FormattableString.Invariant($"System error: {systemErrorCode}"))
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public SystemCallFailedException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public SystemCallFailedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected SystemCallFailedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
