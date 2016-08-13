using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CF.Core.Exceptions;
using static System.FormattableString;

namespace CF.Core.Exceptions.Database
{
	/// <summary>
	/// The exception that is thrown when number of affected rows during SQL query execution differs from expected.
	/// </summary>
	[Serializable]
	public class UnexpectedDbAffectedRowsException : BasicDbException
	{
		/// <summary>
		/// Expected number of affected rows.
		/// </summary>
		public int Expected { get; set; }

		/// <summary>
		/// Actual number of affected rows.
		/// </summary>
		public int Affected { get; set; }

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
		public UnexpectedDbAffectedRowsException(string sqlQuery, int expected, int affected)
			: this(Invariant($"Bad number of affected rows: {affected} != {expected} for '{sqlQuery}'"))
		{
			Expected = expected;
			Affected = affected;
		}

		/// <summary>
		/// Implementation for ISerializable.GetObjectData()
		/// </summary>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}

			info.AddValue(nameof(Expected), Expected);
			info.AddValue(nameof(Affected), Affected);

			base.GetObjectData(info, context);
		}
	}
}
