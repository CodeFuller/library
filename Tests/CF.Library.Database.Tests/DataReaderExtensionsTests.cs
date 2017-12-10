using System;
using System.Data;
using CF.Library.Database.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CF.Library.Database.Tests
{
	[TestClass]
	public class DataReaderExtensionsTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadSafe_CalledForNullObject_ThrowsArgumentNullException()
		{
			DataReaderExtensions.ReadSafe(null);
		}

		[TestMethod]
		[ExpectedException(typeof(NoFetchedDbDataException))]
		public void ReadSafe_CalledForReaderWithoutData_ThrowsNoFetchedDbDataException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(false);

			reader.ReadSafe();
		}

		[TestMethod]
		public void ReadSafe_CalledForReaderWithData_AdvancesReaderToNextRecord()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(true);

			reader.ReadSafe();

			reader.Received(1).Read();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadScalar_CalledForNullObject_ThrowsArgumentNullException()
		{
			DataReaderExtensions.ReadScalar<int>(null);
		}

		[TestMethod]
		[ExpectedException(typeof(NoFetchedDbDataException))]
		public void ReadScalar_CalledForReaderWithNoData_ThrowsNoFetchedDbDataException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(false);

			reader.ReadScalar<int>();
		}

		[TestMethod]
		[ExpectedException(typeof(ExtraDbDataException))]
		public void ReadScalar_CalledForReaderWithMultipleRecords_ThrowsExtraDbDataException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(true);
			reader.FieldCount.Returns(1);
			reader.GetValue(0).Returns(777);

			reader.ReadScalar<int>();
		}

		[TestMethod]
		[ExpectedException(typeof(ScalarDbDataExpectedException))]
		public void ReadScalar_CalledForRecordWithNoFields_ThrowsScalarDbDataExpectedException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(true);
			reader.FieldCount.Returns(0);

			reader.ReadScalar<int>();
		}

		[TestMethod]
		[ExpectedException(typeof(ScalarDbDataExpectedException))]
		public void ReadScalar_CalledForRecordWithMultipleFields_ThrowsScalarDbDataExpectedException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(true);
			reader.FieldCount.Returns(2);

			reader.ReadScalar<int>();
		}

		[TestMethod]
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
