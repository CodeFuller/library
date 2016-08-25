using System;
using System.Runtime.Serialization;
using static System.FormattableString;

namespace CF.Core.Exceptions.Database
{
	/// <summary>
	/// The exception that is thrown when query was expected to return scalar value but record with no or multiple fields was returned.
	/// </summary>
	[Serializable]
	public class ScalarDbDataExpectedException : BasicDbException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ScalarDbDataExpectedException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ScalarDbDataExpectedException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ScalarDbDataExpectedException(int fetchedFieldCount)
			: this(Invariant($"Scalar was expected but {fetchedFieldCount} fields were fetched"))
		{

		}


		/// <summary>
		/// Constructor
		/// </summary>
		public ScalarDbDataExpectedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected ScalarDbDataExpectedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
