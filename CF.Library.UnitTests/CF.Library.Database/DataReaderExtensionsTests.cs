using System;
using System.Data;
using CF.Library.Database;
using CF.Library.Database.Exceptions;
using NSubstitute;
using NUnit.Framework;

namespace CF.Library.UnitTests.CF.Library.Database
{
	[TestFixture]
	public class DataReaderExtensionsTests
	{
		[Test]
		public void ReadSafe_CalledForNullObject_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => DataReaderExtensions.ReadSafe(null));
		}

		[Test]
		public void ReadSafe_CalledForReaderWithoutData_ThrowsNoFetchedDbDataException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(false);

			Assert.Throws<NoFetchedDbDataException>(() => reader.ReadSafe());
		}

		[Test]
		public void ReadSafe_CalledForReaderWithData_AdvancesReaderToNextRecord()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(true);

			reader.ReadSafe();

			reader.Received(1).Read();
		}

		[Test]
		public void ReadScalar_CalledForNullObject_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => DataReaderExtensions.ReadScalar<int>(null));
		}

		[Test]
		public void ReadScalar_CalledForReaderWithNoData_ThrowsNoFetchedDbDataException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(false);

			Assert.Throws<NoFetchedDbDataException>(() => reader.ReadScalar<int>());
		}

		[Test]
		public void ReadScalar_CalledForReaderWithMultipleRecords_ThrowsExtraDbDataException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(true);
			reader.FieldCount.Returns(1);
			reader.GetValue(0).Returns(777);

			Assert.Throws<ExtraDbDataException>(() => reader.ReadScalar<int>());
		}

		[Test]
		public void ReadScalar_CalledForRecordWithNoFields_ThrowsScalarDbDataExpectedException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(true);
			reader.FieldCount.Returns(0);

			Assert.Throws<ScalarDbDataExpectedException>(() => reader.ReadScalar<int>());
		}

		[Test]
		public void ReadScalar_CalledForRecordWithMultipleFields_ThrowsScalarDbDataExpectedException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(true);
			reader.FieldCount.Returns(2);

			Assert.Throws<ScalarDbDataExpectedException>(() => reader.ReadScalar<int>());
		}

		[Test]
		public void ReadScalar_CalledForScalarResult_ReturnsCorrectValue()
		{
			const int testValue = 777;
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(true, false);
			reader.FieldCount.Returns(1);
			reader.GetValue(0).Returns(testValue);

			int returnedValue = reader.ReadScalar<int>();

			Assert.AreEqual(testValue, returnedValue);
		}
	}
}
