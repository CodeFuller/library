using System;
using System.Runtime.Serialization;

namespace CF.Library.Database.Exceptions
{
	/// <summary>
	/// The exception that is thrown when number of affected rows during SQL query execution differs from expected.
	/// </summary>
	[Serializable]
	public class UnexpectedDbAffectedRowsException : BasicDbException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedDbAffectedRowsException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedDbAffectedRowsException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedDbAffectedRowsException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected UnexpectedDbAffectedRowsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedDbAffectedRowsException(string sqlQuery, int affected)
			: this(FormattableString.Invariant($"Bad number of affected rows: {affected} for '{sqlQuery}'"))
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedDbAffectedRowsException(string sqlQuery, int expected, int affected)
			: this(FormattableString.Invariant($"Bad number of affected rows: {affected} != {expected} for '{sqlQuery}'"))
		{
		}
	}
}
