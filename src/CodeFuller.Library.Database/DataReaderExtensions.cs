using System;
using System.Data;
using CodeFuller.Library.Database.Exceptions;

namespace CodeFuller.Library.Database
{
	/// <summary>
	/// Holder for IDataReader extension methods.
	/// </summary>
	public static class DataReaderExtensions
	{
		/// <summary>
		/// Reads data and asserts that data presents.
		/// </summary>
		public static void ReadSafe(this IDataReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException(nameof(reader));
			}

			if (!reader.Read())
			{
				throw new NoFetchedDbDataException();
			}
		}

		/// <summary>
		/// Reads scalar value and asserts that actually scalar was returned.
		/// </summary>
		public static T ReadScalar<T>(this IDataReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException(nameof(reader));
			}

			reader.ReadSafe();

			if (reader.FieldCount != 1)
			{
				throw new ScalarDbDataExpectedException(reader.FieldCount);
			}

			T value = (T)reader.GetValue(0);

			DbAssert.NoMoreData(reader);

			return value;
		}
	}
}
