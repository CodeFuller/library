﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using CF.Core.Exceptions.Database;
using NUnit.Framework.Internal;
using CF.Extensions.Database;
using NSubstitute;
using NUnit.Framework;

namespace CF.Library.Tests.CF.Extensions.Database
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
		public void AssertNoMoreData_CalledForNullObject_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => DataReaderExtensions.AssertNoMoreData(null));
		}

		[Test]
		public void AssertNoMoreData_CalledForReaderWithoutData_PassesSuccessfully()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(false);

			Assert.DoesNotThrow(() => reader.AssertNoMoreData());
		}

		[Test]
		public void AssertNoMoreData_CalledForReaderWithData_ThrowsExtraDbDataException()
		{
			IDataReader reader = Substitute.For<IDataReader>();
			reader.Read().Returns(true);

			Assert.Throws<ExtraDbDataException>(() => reader.AssertNoMoreData());
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

			Assert.AreEqual(returnedValue, testValue);
		}
	}
}