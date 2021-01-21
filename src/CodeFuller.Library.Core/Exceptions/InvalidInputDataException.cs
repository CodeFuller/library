using System;
using System.Runtime.Serialization;

namespace CodeFuller.Library.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when input data is invalid.
	/// </summary>
	[Serializable]
	public class InvalidInputDataException : BasicException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public InvalidInputDataException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public InvalidInputDataException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public InvalidInputDataException(string message, string inputData)
			: base($"{message}\n{LineDelimiter}\n{inputData}\n{LineDelimiter}")
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public InvalidInputDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected InvalidInputDataException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		private static string LineDelimiter => new String('-', 10);
	}
}
