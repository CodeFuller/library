using System;
using System.Data;
using CF.Library.Database.Exceptions;

namespace CF.Library.Database
{
	/// <summary>
	/// Static class for database call result assertions.
	/// </summary>
	public static class DbAssert
	{
		/// <summary>
		/// Asserts that there is no more data in the reader.
		/// </summary>
		public static void NoMoreData(IDataReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException(nameof(reader));
			}

			if (reader.Read())
			{
				throw new ExtraDbDataException();
			}
		}

		/// <summary>
		/// Asserts number of affected rows for command execution.
		/// </summary>
		public static void RowsAffected(IDbCommand command, int expectedAffectedRows, int affectedRows)
		{
			if (command == null)
			{
				throw new ArgumentNullException(nameof(command));
			}

			if (affectedRows != expectedAffectedRows)
			{
				throw new UnexpectedDbAffectedRowsException(command.CommandText, expectedAffectedRows, affectedRows);
			}
		}
	}
}
