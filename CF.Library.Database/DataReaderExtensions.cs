using System;
using System.Data;
using CF.Library.Database.Exceptions;

namespace CF.Library.Database
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
		/// Asserts that there is no more data in the reader.
		/// </summary>
		public static void AssertNoMoreData(this IDataReader reader)
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

			reader.AssertNoMoreData();

			return value;
		}
	}
}
