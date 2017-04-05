using System;
using System.Runtime.Serialization;

namespace CF.Library.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when unexpected enum value is encountered.
	/// </summary>
	[Serializable]
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
		public UnexpectedEnumValueException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedEnumValueException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected UnexpectedEnumValueException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public UnexpectedEnumValueException(Enum e)
			: base(FormattableString.Invariant($"Unexpected enum value: {e}"))
		{
		}
	}
}
